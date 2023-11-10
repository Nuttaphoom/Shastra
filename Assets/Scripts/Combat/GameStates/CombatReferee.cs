
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;


namespace Vanaring
{
    public enum ECompetatorSide
    {
        Ally,
        Hostile
    }

    public class CombatReferee : MonoBehaviour   
    {
        private EventBroadcaster _eventBroadcaster;
        public EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<Null>("OnCombatPreparation");    
            }
        
            return _eventBroadcaster; 
        }


        [SerializeField]
        private EnemyHUDWindowManager _enemyHUDWindowManager;

        [SerializeField]
        private EntityLoader _entityLoader;

        [SerializeField]
        private int _maxTeamSize = 3;

        private ECompetatorSide _currentSide; // Assign this to the opposite of the actual turn we want to start with 

        //The active entities will always be the one at index 0
        private CircularArray<CombatEntity> _activeCombatEntities;
        private CombatRefereeStateHandler _combatRefereeStateHandler;

        
        [Serializable]
        private struct CompetatorDetailStruct
        {
            [SerializeField]
            private ECompetatorSide _side;
            [SerializeField]
            private CombatEntity _entity;
            public ECompetatorSide Side => _side;
            public CombatEntity Competator => _entity;
            public CompetatorDetailStruct(ECompetatorSide side, CombatEntity entity)
            {
                _side = side;
                _entity = entity;
            }
        }

        [SerializeField]
        [Header("For testing, we manually assign these competator (including enemy) ")]
        List<CompetatorDetailStruct> _competators;


        public static CombatReferee instance;

        private void Awake()
        {
            instance = this;
            _currentSide = ECompetatorSide.Ally;

            _activeCombatEntities = new CircularArray<CombatEntity>(new List<CombatEntity>() );
            _combatRefereeStateHandler = new CombatRefereeStateHandler(this);
        }

        private void Start()
        {
            StartCoroutine(BeginNewBattle()); 
        }

        #region SettingUpRound
        private IEnumerator BeginNewBattle()
        {
            SetUpNewCombatEncounter();
            _eventBroadcaster.InvokeEvent<Null>(null, "OnCombatPreparation"); //b <Null>("OnCombatPreparation");

            yield return new WaitForSeconds(1.0f) ;

            StartCoroutine(CustomTick());



        }
        private void SetUpNewCombatEncounter()
        {
            AssignCompetators(_entityLoader.LoadData(), ECompetatorSide.Hostile);
        }
        private void AssignCompetators(List<CombatEntity> entites, ECompetatorSide side)
        {
            foreach (var entity in entites)
            {
                CompetatorDetailStruct c = new CompetatorDetailStruct(side, entity);
                _competators.Add(c);
            }

            // Call the GenerateEntityAttacher method with the lists
            CameraSetUPManager.Instance.GenerateEntityAttacher(GetCompetatorsBySide(ECompetatorSide.Ally).Select(c => c.gameObject).ToList(), GetCompetatorsBySide(ECompetatorSide.Hostile).Select(c => c.gameObject).ToList());
        }
        public IEnumerator PrepareRefereeForNewRound()
        {
            //_currentSide = ECompetatorSide.Ally;
            //_currentEntityIndex = 0;

            SetActiveActors(); 
            yield return SwitchControl(null,GetCurrentActor());
        }

        #endregion

        private IEnumerator CustomTick()
        {
            while (true)
            {
                yield return _combatRefereeStateHandler.AdvanceRound();

                _currentSide = (ECompetatorSide)(((int)_currentSide + 1) % 2);

                //TODO : Determine end game condition
                bool GameIsEnd = false;
                if (GameIsEnd)
                    break;

                yield return new WaitForEndOfFrame();
            }
        }

        #region RefereeHandler Methods
        private IEnumerator SwitchControl(CombatEntity prevEntity, CombatEntity newEntity)
        {
            if (prevEntity != null)
            {
                FindObjectOfType<CharacterWindowManager>().DeSetActiveEntityGUI(prevEntity);
                yield return prevEntity.TakeControlLeave();
            }

            if (prevEntity != newEntity && newEntity != null)
            {
                FindObjectOfType<CharacterWindowManager>().SetActiveEntityGUI(newEntity);

                yield return newEntity.TakeControl();

            }
        }

        private IEnumerator ChangeActiveEntityIndexCoroutine(bool forward)
        {
            CombatEntity prevActor = GetCurrentActor();

            _activeCombatEntities.Progress(forward);


            //Changes was made
            if (prevActor != GetCurrentActor())
            {
                yield return SwitchControl(prevActor,GetCurrentActor()); 
            }

            yield return null; 
             
        }
        public bool ChangeActiveEntityIndex(bool forward)
        {
            if (GetCurrentActiveEntities().Count > 1)
            {
                StartCoroutine(ChangeActiveEntityIndexCoroutine(forward));
                return true;
            }
            return false;
        }
        public CombatEntity InstantiateCompetator(CombatEntity prefabNewCompetator, ECompetatorSide side)
        {
            List<CombatEntity> entitesWithSameSide = new List<CombatEntity>();
            entitesWithSameSide = GetCompetatorsBySide(side);

            if (entitesWithSameSide.Count > _maxTeamSize)
                return null ; 
            
            CombatEntity entity = _entityLoader.SpawnPrefab(prefabNewCompetator) ;

            AssignCompetators(new List<CombatEntity>() { entity } , side);

           
            return entity ; 
        }

        #endregion

        #region EntityDOAction Methods

        public IEnumerator OnCharacterPerformAction(CombatEntity actor, ActorAction action)
        {
            yield return SwitchControl(GetCurrentActor(), null) ; 

            var targets = action.GetActionTargets();
            
            yield return actor.OnPerformAction(action);

            yield return PostPerformActionInEveryCharacter();

            ResolveEntityDead(); 

            SetActiveActors();
            
            yield return SwitchControl(null, GetCurrentActor());
        }

        private void ResolveEntityDead()
        {
            //Check for dead entity
            for (int i = _competators.Count - 1; i >= 0; i--)
            {
                if (_competators[i].Competator.IsDead)
                {

                    //No need to remove from the main list if it was player's
                    _entityLoader.ReleasePosition(_competators[i].Competator);
                    _competators.RemoveAt(i);
                }
            }

   

        }
        private IEnumerator PostPerformActionInEveryCharacter()
        {
            foreach (ECompetatorSide side in (ECompetatorSide[])Enum.GetValues(typeof(ECompetatorSide)))
            {
                foreach (var entity in GetCompetatorsBySide(side))
                {
                    yield return entity.OnPostPerformAction();
                }
            }
        }

        #endregion

        /// <summary>
        /// this function should be called everytime an action is finished performed
        /// </summary>
        public void SetActiveActors()
        {
            var team = GetCurrentTeam();

            _activeCombatEntities.Reset();
            for (int i = 0; i < team.Count; i++)
            {
                if (!team[i].ReadyForControl())
                {
                    team.RemoveAt(i);
                    i--;
                    continue;
                }

                _activeCombatEntities.Add(team[i]); 
            }

        } 

        #region GETTER 
        public List<CombatEntity> GetCurrentActiveEntities()
        {
            if (_activeCombatEntities == null)
                throw new Exception("_activeCombatEntities is null"); 
            
            return _activeCombatEntities.GetDataAsList()  ; 
        }
        
        public List<CombatEntity> GetCurrentTeam()
        {
            return GetCompetatorsBySide(_currentSide);
        }

        //Good 
        public List<CombatEntity> GetCompetatorsBySide(ECompetatorSide ESide)
        {
            return _competators.Where(v => v.Side == ESide).Select(v => v.Competator).ToList();
        }

        public CombatEntity GetCurrentActor()
        {
            if (_activeCombatEntities.Count() == 0)
                return null; 

            return _activeCombatEntities[0]; 
        }


        public ECompetatorSide GetCompetatorSide(CombatEntity entity)
        {
            foreach (var competator in _competators)
            {
                if (competator.Competator == entity)
                {
                    return competator.Side;
                }
            }

            throw new Exception("Given entity is not registered in CombatReferee");
        }
        #endregion

    }
}

