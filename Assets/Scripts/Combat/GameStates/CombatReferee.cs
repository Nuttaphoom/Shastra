
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

        #region Event
        
        public UnityAction OnCharacterPerformEvent ;
        
        #endregion
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

        private int _currentEntityIndex = 0;

        private CombatRefereeStateHandler _combatRefereeStateHandler;

        private void Awake()
        {
            instance = this;

            OnCharacterPerformEvent += OnCharacterPerformAction_SwitchControl; 
        }

        private void OnDisable()
        {
            OnCharacterPerformEvent -= OnCharacterPerformAction_SwitchControl;
        }

        private void Start()
        {
            _combatRefereeStateHandler = new CombatRefereeStateHandler(this); 
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
            _currentSide = ECompetatorSide.Ally;
            _currentEntityIndex = 0;

            yield return SwitchControl(-1, _currentEntityIndex);
        }

        #endregion

        private IEnumerator CustomTick()
        {
            yield return _combatRefereeStateHandler.AdvanceRound(); 
        }

        //Need to check
        private IEnumerator SwitchControl(int prev, int next)
        {
            List<CombatEntity> _activeEntities = GetCurrentActiveEntities(); 
            if (prev != -1)
            {
                FindObjectOfType<CharacterWindowManager>().DeSetActiveEntityGUI(_activeEntities[prev]);
                yield return _activeEntities[prev].LeaveControl();
            }

            if (prev != next)
            {
                FindObjectOfType<CharacterWindowManager>().SetActiveEntityGUI(_activeEntities[next]);


                for (int i = 0; i < GetCompetatorsBySide(ECompetatorSide.Ally).Count; i++)
                {
                    if (GetCompetatorsBySide(ECompetatorSide.Ally)[i] == _activeEntities[next])
                    {
                        CameraSetUPManager.Instance.SelectCharacterCamera(i);
                    }

                }

                yield return _activeEntities[next].TakeControl();

            }
        }

        public bool ChangeActiveEntityIndex(bool increase = false, bool decrease = false)
        {
            if (GetCurrentActiveEntities().Count > 1)
            {
                StartCoroutine(ChangeActiveEntityIndexCoroutine(increase, decrease));
                return true;
            }
            return false;
        }

        public IEnumerator ChangeActiveEntityIndexCoroutine(bool increase = false, bool decrease = false)
        {
            Debug.Log("change active entity indexc coroutine"); 
            List<CombatEntity> _activeEntities = GetCurrentActiveEntities();
            
            if (_activeEntities.Count > 0)
            {
                int temp = _currentEntityIndex;

                if (increase)
                    _currentEntityIndex = (_currentEntityIndex + 1) % _activeEntities.Count;
                else if (decrease)
                    _currentEntityIndex = _currentEntityIndex == 0 ? (_activeEntities.Count - 1) : (_currentEntityIndex - 1);

                if (temp != _currentEntityIndex)
                    yield return SwitchControl(temp, _currentEntityIndex);
            }
            else
            {
                _currentEntityIndex = 0;
            }
        }


        #region EventListener  

        private void OnCharacterPerformAction_SwitchControl()
        {
            
        }

        #endregion

        #region GETTER 
        public List<CombatEntity> GetCurrentActiveEntities()
        {
            return GetCurrentTeam().Where(v => v.ReadyForControl() == true).ToList();
        }
        //Good 
        public List<CombatEntity> GetCompetatorsBySide(ECompetatorSide ESide)
        {
            return _competators.Where(v => v.Side == ESide).Select(v => v.Competator).ToList();
        }

        public List<CombatEntity> GetCurrentTeam()
        {
            return GetCompetatorsBySide(_currentSide);
        }

        public CombatEntity GetCurrentActor()
        {
            return GetCurrentTeam()[_currentEntityIndex]; 
        }


        #endregion

    }
}

