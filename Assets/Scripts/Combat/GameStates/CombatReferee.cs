
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
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
        public static CombatReferee instance = null;
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

        private ECompetatorSide _currentSide  ; // Assign this to the opposite of the actual turn we want to start with 

        //The active entities will always be the one at index 0
        private CircularArray<CombatEntity> _activeCombatEntities ; 
        private CombatRefereeStateHandler _combatRefereeStateHandler;

        private void Awake()
        {
            instance = this;
            _currentSide = ECompetatorSide.Ally;

            _activeCombatEntities = new CircularArray<CombatEntity>(new List<CombatEntity>() );
            _combatRefereeStateHandler = new CombatRefereeStateHandler(this);
        }
        private void Start()
        {
            SetUpNewCombatEncounter();

            StartCoroutine(CustomTick());
        }

        #region SettingUpRound
        private void SetUpNewCombatEncounter()
        {
            AssignCompetators(FindObjectOfType<EntityLoader>().LoadData(), ECompetatorSide.Hostile);
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

        private IEnumerator SwitchControl(CombatEntity prevEntity, CombatEntity newEntity)
        {
            if (prevEntity != null)
            {
                FindObjectOfType<CharacterWindowManager>().DeSetActiveEntityGUI(prevEntity);
                yield return prevEntity.LeaveControl();
            }

            if (prevEntity != newEntity && newEntity != null)
            {
                FindObjectOfType<CharacterWindowManager>().SetActiveEntityGUI(newEntity);

                for (int i = 0; i < GetCompetatorsBySide(ECompetatorSide.Ally).Count; i++)
                {
                    if (GetCompetatorsBySide(ECompetatorSide.Ally)[i] == newEntity)
                    {
                        CameraSetUPManager.Instance.SelectCharacterCamera(i);
                    }
                }
                
                yield return newEntity.TakeControl();

            }
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

        public IEnumerator OnCharacterPerformAction()
        {
            CombatEntity _combatEntity = GetCurrentActor(); 
            SetActiveActors();
            yield return SwitchControl(_combatEntity, GetCurrentActor()); 
        }

        /// <summary>
        /// this function should be called everytime an action is finished performed
        /// </summary>
        private void SetActiveActors()
        {
            var team = GetCurrentTeam();

            foreach (var v in team)
            {
                Debug.Log(v); 
            }

            _activeCombatEntities.Reset();

            for (int i =0; i < team.Count; i++)
            {
                if (! team[i].ReadyForControl())
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

