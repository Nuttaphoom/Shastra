
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
using UnityEngine; 
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.CanvasScaler;
using Vanaring.Assets.Scripts.Utilities;


namespace Vanaring
{
    public enum ECompetatorSide
    {
        Ally,
        Hostile
    }

    public class CombatReferee : MonoBehaviour
    {

        [Header("Competators for debugging")]
        [SerializeField ]
        List<CompetatorDetailStruct> _competators;
        #region EventBroadcaster
        private EventBroadcaster _eventBroadcaster;

        [SerializeField]
        private CharacterEntityPrefabDatabaseSO _testAllyEntityLoaderPrefab; 

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<Null>("OnCombatPreparation");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnCompetitorEnterCombat");
                _eventBroadcaster.OpenChannel<ECompetatorSide>("OnNewRoundBegin");
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

        public void SubOnNewRoundBegin(UnityAction<ECompetatorSide> argc)
        {
            GetEventBroadcaster().SubEvent<ECompetatorSide>(argc, "OnNewRoundBegin");
        }

        public void UnSubOnNewRoundBegin(UnityAction<ECompetatorSide> argc)
        {
            GetEventBroadcaster().UnSubEvent<ECompetatorSide>(argc, "OnNewRoundBegin");
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
            public ECompetatorSide Side;
            [SerializeField]
            public CombatEntity Competator;
    
            public CompetatorDetailStruct(ECompetatorSide side, CombatEntity entity)
            {
                Side = side;
                Competator = entity;
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
            if (EnableDebuggingChecker.Instance.IsDebugingModeEnable)
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
                ////Unused data for debuging mode is need be clear
                //if (_competators.Count > 0)
                //{
                //    for (int i = 0; i < _competators.Count; i++)
                //    {
                //        Destroy(_competators[i].Competator.gameObject);
                //        _competators.RemoveAt(i);
                //        i--;
                //    }
                //    _competators.Clear();
                //}



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
            
            List<CombatEntity> entities = new List<CombatEntity>();

            ////test loading from Address 
            //var prefab = Resources.Load<CombatEntity>("Ally Prefabs/Asha-Entity Ally Prefab");  /*PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>("Asha_Entity_Prefab");*/

            //CombatEntity newEntity = Instantiate(prefab); 

            ////newEntity.GetComponent<ControlableEntity>().LinkPartyMemberToThisEntity( );
            //entities.Add(newEntity ); 

            foreach (var prefab in _testAllyEntityLoaderPrefab.GetAllControlableEntiites)
            {
                foreach (RuntimePartyMember partyMember in playerParty)
                {
                    if (partyMember.GetCharacterSheet.CharacterName == prefab.CombatCharacterSheet.CharacterName)
                    {
                        CombatEntity newEntity = Instantiate(prefab);
                        entities.Add(newEntity);
                        (newEntity as ControlableEntity).LinkPartyMemberToThisEntity(partyMember);
                        break;
                    }
                }
            }
            //Load controlable entities from Party data 
            //1.)
            //foreach (RuntimePartyMember partyMember in playerParty)
            //{
            //    CombatEntity newEntity = Instantiate(partyMember.GetCharacterSheet.GetCombatEntityPrefab);
            //    newEntity.GetComponent<ControlableEntity>().LinkPartyMemberToThisEntity(partyMember);
            //    entities.Add(newEntity);
            //}

            //2.)
            //foreach (CombatEntity entity in FindObjectsOfType<CombatEntity>())
            //{
            //    var newEntity = Instantiate(entity);
            //    entities.Add(newEntity);
            //}


            ////Test : instanite from Competators 
            //foreach (var en in _competators)
            //{
            //    if (en.Side == ECompetatorSide.Ally)
            //    {
            //        foreach (RuntimePartyMember partyMember in playerParty)
            //        {
            //            if (partyMember.GetCharacterSheet.CharacterName == en.Competator.CombatCharacterSheet.CharacterName)
            //            {
            //                (en.Competator as ControlableEntity).LinkPartyMemberToThisEntity(partyMember);
            //            }
            //        }
            //        entities.Add(Instantiate(en.Competator));
            //    }
            //}

            //Unused data for debuging mode is need be clear
            if (_competators.Count > 0)
            {
                for (int i = 0; i < _competators.Count; i++)
                {
                    //if (_competators[i].Side == ECompetatorSide.Ally)
                    //{
                    //    entities.Add(_competators[i].Competator); 
                    //}
                    Destroy(_competators[i].Competator.gameObject);
                    _competators.RemoveAt(i);
                    i--;
                }
                _competators.Clear();
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
        private IEnumerator AssignCompetators(List<CombatEntity> entites, ECompetatorSide side, bool addDuringCombat = false)
        {

            List<IEnumerator> _allIEs = new List<IEnumerator>();

            if (side == ECompetatorSide.Hostile)
            {
                int newEnemySize = entites.Count ; 
                if (addDuringCombat)
                {
                    newEnemySize = EntityPositionManager.Instance.CurrentEnemySize + entites.Count; 
                }
                 
                EntityPositionManager.Instance.SetNewEnemyCurrentSize(newEnemySize);

            }

            if (side == ECompetatorSide.Ally)
            {
                foreach (var entity in entites)
                {
                    entity.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh(); 
                }
            }


            for (int i = 0; i < entites.Count; i++)
            {
                CompetatorDetailStruct c = new CompetatorDetailStruct(side, entites[i]);
                _competators.Add(c);

                _allIEs.Add(entites[i].InitializeEntityIntoCombat());

            }

            HanderRefereeOrder();

            EntityPositionManager.Instance.ReleaseAllPosition(); 

            for (int i =0; i< GetCompetatorsBySide(side).Count; i++)
            {
                var e = GetCompetatorsBySide(side);
                //ColorfulLogger.LogWithColor("IDK WHY THIS IS CCALLED", Color.yellow);
                EntityPositionManager.Instance.OccupieLocation(side, i, e[i]);
            }

            yield return new WaitAll(this, _allIEs.ToArray());


            foreach (var entity in entites)
            {
 

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
            

            yield return SetActiveActors(); 
            yield return SwitchControl(null,GetCurrentActor());

            EntityPositionManager.Instance.SetNewEnemyCurrentSize(GetCompetatorsBySide(ECompetatorSide.Hostile).Count);


        }

        #endregion

        private IEnumerator CustomTick()
        {
            while (true)
            {
                yield return _sideTurnDisplayerManager.DisplaySideRoundCoroutine(_currentSide);


                yield return _combatRefereeStateHandler.StateEnter();

                GetEventBroadcaster().InvokeEvent<ECompetatorSide>(_currentSide, "OnNewRoundBegin");


                yield return _combatRefereeStateHandler.AdvanceRound();

                _currentSide = (ECompetatorSide)(((int)_currentSide + 1) % 2);



                yield return new WaitForEndOfFrame();
            }
        }


        public bool IsGameEnd()
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

            if (/*prevEntity != newEntity &&*/ newEntity != null)
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

        public IEnumerator InstantiateCompetator(List<CombatEntity> prefabNewCompetator, ECompetatorSide side, bool addDurningCombat = false)
        {
            List<CombatEntity> entitesWithSameSide = new List<CombatEntity>();
            entitesWithSameSide = GetCompetatorsBySide(side);

            if (entitesWithSameSide.Count > _maxTeamSize)
                throw new Exception("Can't spawn more competator into the combat");

            List<CombatEntity> newSpawnEnitites = new List<CombatEntity>(); 
            foreach (var prefab in prefabNewCompetator)
            {
                newSpawnEnitites.Add(_entityLoader.SpawnPrefab(prefab) );
            }
            yield return AssignCompetators(newSpawnEnitites, side, addDurningCombat);

           
             
        }

        #endregion

        #region EntityDOAction Methods

        public IEnumerator OnCharacterPerformAction(CombatEntity actor )
        {
            yield return actor.OnPerformAction( );

            ResolveEntityDead();

            yield return PostPerformActionInEveryCharacter();


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

                yield return SetActiveActors();

                //Debug.Log("switch control to " + GetCurrentActor()); 
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
                    //ColorfulLogger.LogWithColor("Resolve Trigger ! ", Color.red);

                    yield return entity.OnPostPerformAction();
                }
            }
        }

        #endregion

        /// <summary>
        /// this function should be called everytime an action is finished performed
        /// </summary>
        public IEnumerator SetActiveActors()
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

            for (int i = 0; i < _activeCombatEntities.Count(); i++)
            {
                if (_activeCombatEntities[i].WasGetForcedRelieved(true))
                {
                    int repeatition = 0;
                    while (!_activeCombatEntities[0].WasGetForcedRelieved(true))
                    {
                        repeatition++;
                        if (repeatition > 10)
                            throw new Exception("repeatition exceeed 10");

                        _activeCombatEntities.Progress(true); 
                    }
                    //Debug.Log("exit loop with " + _activeCombatEntities[0] + " at 0");
                }
            }

            //if (_activeCombatEntities.Count() > 0)
            //{
            //    //If there is one active character, handle referee order 
            //    //HanderRefereeOrder();

            //}

            ////search for force relieve entities 
            //for (int i =0 ; i < _activeCombatEntities.Count() ; i++)
            //{
            //    if (_activeCombatEntities[i].WasGetForcedRelieved())
            //    {

            //        //var temp = _activeCombatEntities[0];
            //        _activeCombatEntities[0] = _activeCombatEntities[i];
            //        //_activeCombatEntities[i] = temp;
            //        break; 
            //    }
            //}

            //Debug.Log("Exit loop with 0 is " + _activeCombatEntities[0].gameObject);

            yield return null ;

        }
        private void HanderRefereeOrder()
        {
            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                bool changeIndex = false;
                var entities = GetCompetatorsBySide(side);
                for (int i = 0; i < entities.Count; i++)
                {

                    if (!entities[i].CombatEntityAnimationHandler.DedicateToLocation)
                        continue;

                    if (entities[i].CombatEntityAnimationHandler.GetDedicateLocation == i)
                        continue;

                    if (entities[i].CombatEntityAnimationHandler.GetDedicateLocation >= entities.Count)
                        continue;


                    var e = _competators[i];

                    changeIndex = true;

                    var temp = entities[i];
                    entities[i] = entities[entities[i].CombatEntityAnimationHandler.GetDedicateLocation];
                    entities[temp.CombatEntityAnimationHandler.GetDedicateLocation] = temp;

                }

                if (changeIndex)
                {

                    for (int i = 0; i < _competators.Count; i++)
                    {
                        if (_competators[i].Side == side)
                        {
                            _competators.RemoveAt(i);
                            i--;
                        }
                    }

                    for (int i = 0; i < entities.Count; i++)
                    {
                        _competators.Add(new CompetatorDetailStruct(side, entities[i]));
                    }

                   
                }

                if (side == ECompetatorSide.Hostile)
                {
                    EntityPositionManager.Instance.SetNewEnemyCurrentSize(entities.Count);
                }
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

