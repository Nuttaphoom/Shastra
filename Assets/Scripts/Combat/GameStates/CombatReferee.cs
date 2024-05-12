
using CustomYieldInstructions;
using JetBrains.Annotations;
using NaughtyAttributes;
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
using Vanaring.Assets.Scripts.Combat.Utilities;
using Vanaring_Utility_Tool;
using static UnityEngine.EventSystems.EventTrigger;
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
        [SerializeField]
        private bool _OnDebugMode;
        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("_OnDebugMode")]
        List<CompetatorDetailStruct> _competators;
        #region EventBroadcaster
        private EventBroadcaster _eventBroadcaster;

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<Null>("OnCombatPreparation");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnCompetitorEnterCombat");
                _eventBroadcaster.OpenChannel<Null>("OnNewRoundBegin");
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

        public void SubOnNewRoundBegin(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent<Null>(argc, "OnNewRoundBegin");
        }

        public void UnSubOnNewRoundBegin(UnityAction<Null> argc)
        {
            GetEventBroadcaster().UnSubEvent<Null>(argc, "OnNewRoundBegin");
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

        private SideTurnDisplayerManager _sideTurnDisplayerManager ;

        
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



        private static CombatReferee _instance; 
        public static CombatReferee Instance {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<CombatReferee>() ; 

                return _instance;
            }
        } 
       
        private void Awake()
        {
            _instance = this;

            _sideTurnDisplayerManager = FindObjectOfType<SideTurnDisplayerManager>();  
            if (_sideTurnDisplayerManager == null)
                throw new Exception("SideTurnDisplayerManager CANNOT be found"); 

            _currentSide = ECompetatorSide.Ally;
            _activeCombatEntities = new CircularArray<CombatEntity>(new List<CombatEntity>());
            _combatRefereeStateHandler = new CombatRefereeStateHandler(this);
        }

         

        #region SettingUpRound
        public IEnumerator InitializeCombat(List<RuntimePartyMember> playerParty)
        {
            if (_OnDebugMode)
            {
                List<CombatEntity> entities = new List<CombatEntity>();
                foreach (var en in _competators)
                {
                    if (en.Side == ECompetatorSide.Ally)
                        entities.Add(en.Competator) ; 
                }
                _competators.Clear(); 

                yield return DebugMode_LoadAllyEntityRuntimeData(entities);
 

            }else
            {
                //Unused data for debuging mode is need be clear
                if (_competators.Count > 0)
                {
                    for (int i = 0; i < _competators.Count; i++)
                    {
                        Destroy(_competators[i].Competator.gameObject);
                        _competators.RemoveAt(i);
                        i--;
                    }
                    _competators.Clear();
                }



                //Load party member into combat
                yield return LoadAllyEntityRuntimeData(playerParty);
                //Set up party member and inventory from database
                yield return LoadDataFromDatabase();
            }

            yield return SetUpNewCombatEncounter();

        }

        public void BeginNewBattle()
        {
            //yield return new WaitForSeconds(1.0f);

            GetEventBroadcaster().InvokeEvent<Null>(null, "OnCombatPreparation"); //b <Null>("OnCombatPreparation

            StartCoroutine(CustomTick());

        }

        private IEnumerator LoadAllyEntityRuntimeData(List<RuntimePartyMember> playerParty)
        {
            //Load controlable entities from Party data 
            List<CombatEntity> entities = new List<CombatEntity>();
            foreach (RuntimePartyMember partyMember in playerParty)
            {
                ControlableEntity newEntity = partyMember.InitializeCombatEntity as ControlableEntity ;
                newEntity.LinkPartyMemberToThisEntity(partyMember); 
                entities.Add(newEntity) ;
            }


            yield return AssignCompetators(entities, ECompetatorSide.Ally); 
        }

        private IEnumerator DebugMode_LoadAllyEntityRuntimeData(List<CombatEntity> playerControlableEntities)
        {
             
            //Load controlable entities from Party data 
            List<CombatEntity> entities = new List<CombatEntity>();
            foreach (ControlableEntity entity in playerControlableEntities)
            {
                //entity.LinkPartyMemberToThisEntity(partyMember);
                entities.Add(entity);
            }


            yield return AssignCompetators(entities, ECompetatorSide.Ally);
        }
        private IEnumerator LoadDataFromDatabase()
        {

            foreach (ICombatRequireLoadData icombatRequireLoadData in FindObjectsOfType<MonoBehaviour>()
                                                                    .OfType<ICombatRequireLoadData>())
            {
                yield return icombatRequireLoadData.LoadDataFromDatabase();
            }

           
            yield return null;
        }

       
        private IEnumerator SetUpNewCombatEncounter()
        {
            // Call the GenerateEntityAttacher method with the lists
            CameraSetUPManager.Instance.GenerateEntityAttacher(GetCompetatorsBySide(ECompetatorSide.Ally).Select(c => c.gameObject).ToList(), GetCompetatorsBySide(ECompetatorSide.Hostile).Select(c => c.gameObject).ToList());

            yield return AssignCompetators(_entityLoader.GetCombatEntitiesFromPool(), ECompetatorSide.Hostile);
        }

        /// <summary>
        /// Use for property set up entities for the combat, this include adding them into _competators list 
        /// and set up position, 
        /// Entities => every entity in that team 
        /// </summary>
        /// <param name="entites"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        private IEnumerator AssignCompetators(List<CombatEntity> entites, ECompetatorSide side)
        {
            List<IEnumerator> _allIEs = new List<IEnumerator>();

            if (side == ECompetatorSide.Hostile)
                EntityPositionManager.Instance.SetNewEnemyCurrentSize(entites.Count); 
            

            foreach (var entity in entites) {
                EntityPositionManager.Instance.OccupieAnyValidLocation(side, entity); 
                _allIEs.Add(entity.InitializeEntityIntoCombat());
            }

            if (side == ECompetatorSide.Ally)
            {
                foreach (var entity in entites)
                {
                    entity.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh(); 
                }
            }

            yield return new WaitAll(this, _allIEs.ToArray());

            foreach (var entity in entites)
            {
                CompetatorDetailStruct c = new CompetatorDetailStruct(side, entity);
                _competators.Add(c);

                GetEventBroadcaster().InvokeEvent<CombatEntity>(entity, "OnCompetitorEnterCombat");

            }

           

            yield return null; 
        }
        /// <summary>
        /// Adjust position and give control to the first index
        /// </summary>
        /// <returns></returns>
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
                yield return _sideTurnDisplayerManager.DisplaySideRoundCoroutine(_currentSide);

                yield return _combatRefereeStateHandler.StateEnter(); 

                GetEventBroadcaster().InvokeEvent<Null>(null,"OnNewRoundBegin");

                yield return _combatRefereeStateHandler.AdvanceRound();

                _currentSide = (ECompetatorSide)(((int)_currentSide + 1) % 2);



                yield return new WaitForEndOfFrame();
            }
        }

        private bool IsGameEnd()
        {
            for (int i = 0 ; i < GetCompetatorsBySide(ECompetatorSide.Ally).Count; i++)
            {
                var entity = GetCompetatorsBySide(ECompetatorSide.Ally)[i];
                if (! entity.IsDead)
                    break;

                if (i == GetCompetatorsBySide(ECompetatorSide.Ally).Count - 1)
                    return true; 
            }

            for (int i = 0; i < GetCompetatorsBySide(ECompetatorSide.Hostile).Count; i++)
            {
                var entity = GetCompetatorsBySide(ECompetatorSide.Hostile)[i];
                if (!entity.IsDead)
                    break;

                if (i == GetCompetatorsBySide(ECompetatorSide.Hostile).Count - 1)
                    return true;
            }

            if (GetCompetatorsBySide(ECompetatorSide.Hostile).Count == 0)
                return true;
            return false;
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

        public IEnumerator OnCharacterPerformAction(CombatEntity actor )
        {

            yield return actor.OnPerformAction( );

            yield return PostPerformActionInEveryCharacter();

            ResolveEntityDead();

            yield return CheckForReactionAction(); 

        }

        private IEnumerator CheckForReactionAction()
        {
            List<CombatEntity> allEntities = new List<CombatEntity>();
            foreach (var entity in GetCompetatorsBySide(ECompetatorSide.Ally))
                allEntities.Add(entity);

            foreach (var entity in GetCompetatorsBySide(ECompetatorSide.Hostile))
                allEntities.Add(entity);


            foreach (var entity in allEntities)
            {
                //yield return new WaitForSeconds(1.0f);

                if (entity.IsDead)
                    continue;

                if (!entity.ActionHandler.ActionQueueReady())
                    continue;


                yield return OnCharacterPerformAction(entity);

                break;
            }
        }

        public IEnumerator ResolveOnEntityPerformAction()
        {
            var prevActor = GetCurrentActor();

            if (IsGameEnd())
            {
                yield return FindObjectOfType<CombatRewardManager>().CombatRewardSchemeStart(this);
                PersistentSceneLoader.Instance.LoadGeneralScene(PersistentSceneLoader.Instance.GetStackLoadedDataScene(1));
                //FindObjectOfType<ThanksForPlayingDisplayer>().ShowThankForPlayingMenu();
            }
            else
            {
                //yield return SwitchControl((), null);

                SetActiveActors();

                yield return SwitchControl(prevActor, GetCurrentActor());

                //if (GetCurrentActor() != null)
                //    EntityPositionManager.Instance.SetNewEnemyCurrentSize(GetCompetatorsBySide(ECompetatorSide.Hostile).Count);


            }
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

                    EntityPositionManager.Instance.ReleasePosition(_competators[i].Competator);
                    
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

