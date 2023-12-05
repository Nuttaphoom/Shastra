
using CustomYieldInstructions;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Numerics;
using TMPro;
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
        #region EventBroadcaster
        private EventBroadcaster _eventBroadcaster ;
        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<Null>("OnCombatPreparation");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnCompetitorEnterCombat");
                
            }

            return _eventBroadcaster; 
        }
        
        public void SubOnCombatPreparation(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent<Null>(argc, "OnCombatPreparation") ;
        }

        public void UnSubOnCombatPreparation(UnityAction<Null> argc)
        {
            GetEventBroadcaster().UnSubEvent<Null>(argc, "OnCombatPreparation");
        }
        public void SubOnCompetitorEnterCombat(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().SubEvent<CombatEntity>(argc, "OnCompetitorEnterCombat");
        }
     
        public void UnSubOnCompetitorEnterCombat(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().UnSubEvent<CombatEntity>(argc, "OnCompetitorEnterCombat"); 
        }
        #endregion

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


        public static CombatReferee Instance ;
       
        private void Awake()
        {
            Instance = this; 

            _currentSide = ECompetatorSide.Ally;
            _activeCombatEntities = new CircularArray<CombatEntity>(new List<CombatEntity>());
            _combatRefereeStateHandler = new CombatRefereeStateHandler(this);
        }

        private void Start()
        {
            StartCoroutine(BeginNewBattle()); 
        }

        #region SettingUpRound
        private IEnumerator BeginNewBattle()
        {

            yield return SetUpNewCombatEncounter();

            yield return new WaitForSeconds(1.0f);

            GetEventBroadcaster().InvokeEvent<Null>(null, "OnCombatPreparation"); //b <Null>("OnCombatPreparation");


            StartCoroutine(CustomTick());



        }
        private IEnumerator SetUpNewCombatEncounter()
        {

            // Call the GenerateEntityAttacher method with the lists
            CameraSetUPManager.Instance.GenerateEntityAttacher(GetCompetatorsBySide(ECompetatorSide.Ally).Select(c => c.gameObject).ToList(), GetCompetatorsBySide(ECompetatorSide.Hostile).Select(c => c.gameObject).ToList());


            foreach (CombatEntity entity in GetCompetatorsBySide(ECompetatorSide.Ally))
                GetEventBroadcaster().InvokeEvent<CombatEntity>(entity, "OnCompetitorEnterCombat");

            yield return AssignCompetators(_entityLoader.LoadData(), ECompetatorSide.Hostile);
        }

        private IEnumerator AssignCompetators(List<CombatEntity> entites, ECompetatorSide side)
        {
            List<IEnumerator> _allIEs = new List<IEnumerator>();

            foreach (var entity in entites) {
                GetEventBroadcaster().InvokeEvent<CombatEntity>(entity, "OnCompetitorEnterCombat");
                _allIEs.Add(entity.InitializeEntityIntoCombat() ) ; 
            }

            yield return new WaitAll(this, _allIEs.ToArray()); 

            foreach (var entity in entites)
            {
                CompetatorDetailStruct c = new CompetatorDetailStruct(side, entity);
                _competators.Add(c);
            }

            
            
            yield return null; 
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
        public IEnumerator InstantiateCompetator(CombatEntity prefabNewCompetator, ECompetatorSide side)
        {
            List<CombatEntity> entitesWithSameSide = new List<CombatEntity>();
            entitesWithSameSide = GetCompetatorsBySide(side);

            if (entitesWithSameSide.Count > _maxTeamSize)
                throw new Exception("Can't spawn more competator into the combat"); 
            
            CombatEntity entity = _entityLoader.SpawnPrefab(prefabNewCompetator) ;

            yield return AssignCompetators(new List<CombatEntity>() { entity } , side);

           
             
        }

        #endregion

        #region EntityDOAction Methods

        public IEnumerator OnCharacterPerformAction(CombatEntity actor, ActorAction action)
        {
            yield return SwitchControl(GetCurrentActor(), null) ; 

            yield return actor.OnPerformAction(action);

            yield return PostPerformActionInEveryCharacter();

            ResolveEntityDead(); 

            if (CombatEnd() == 1)
                MockUpGameOverDisplay.Instance.GameWinDisplay(); 
            else if (CombatEnd() == 2)
                MockUpGameOverDisplay.Instance.GameOverDisplay(); 
            

            SetActiveActors();
            
            yield return SwitchControl(null, GetCurrentActor());

        }

        private int CombatEnd()
        {
            for (int i = 0;  i< GetCompetatorsBySide(ECompetatorSide.Ally).Count; i++)
            {
                var comp = GetCompetatorsBySide(ECompetatorSide.Ally)[i];

                if (!comp.IsDead)
                {
                    break;
                }

                else if (i == GetCompetatorsBySide(ECompetatorSide.Ally).Count - 1)
                    return 2; 

            }

            if (GetCompetatorsBySide(ECompetatorSide.Hostile).Count == 0)
                return  1;  

            return 0 ; 
        }

        private void ResolveEntityDead()
        {
            //Check for dead entity
            for (int i = _competators.Count - 1; i >= 0; i--)
            {
                if (_competators[i].Competator.IsDead)
                {                    
                    //No need to remove from the main list if it was player'
                    if (_competators[i].Side == ECompetatorSide.Ally)
                        continue; 

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

