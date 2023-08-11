
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;


namespace Vanaring_DepaDemo
{
    public enum ECompetatorSide
    {
        Ally,
        Hostile
    }

    public class CombatReferee : MonoBehaviour, IInputReceiver
    {
        private enum CombatState
        {
            Init,
            WaitingForAction,
            Action,
        }
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

        private ECompetatorSide _currentSide = ECompetatorSide.Hostile; // Assign this to the opposite of the actual turn we want to start with 
        private List<CombatEntity> _activeEntities = new List<CombatEntity>();
        private int _currentEntityIndex = 0;

        private CombatState _state = CombatState.Init ; 

        private void Awake()
        {
            _state = CombatState.Init ;
            instance = this;
        }

        private void Start()
        {
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);

            SetUpNewCombatEncounter();
            StartCoroutine(CustomTick());
        }

        public void AssignCompetators(List<CombatEntity> entites, ECompetatorSide side)
        {
            foreach (var entity in entites)
            {
                CompetatorDetailStruct c = new CompetatorDetailStruct(side, entity);
                _competators.Add(c);
            }

            // Call the GenerateEntityAttacher method with the lists
            GameSceneSetUPManager.Instance.GenerateEntityAttacher(GetCompetatorsBySide(ECompetatorSide.Ally).Select(c => c.gameObject).ToList(), GetCompetatorsBySide(ECompetatorSide.Hostile).Select(c => c.gameObject).ToList());

        }

        private IEnumerator CustomTick()
        {
            while (!IsCombatEnded())
            {
                if (_activeEntities.Count <= 0)
                {
                    SetupNewRound();
                }

                yield return AdvanceTurn();
            }
        }
        private void SetupNewRound()
        {
            _currentSide = (ECompetatorSide)(((int)_currentSide + 1) % 2);

            _activeEntities = GetCompetatorsBySide(_currentSide);
        }

        private bool EndGameConditionMeet()
        {
            //Check for enemy first 
            int allyAlive = 0;
            int hostileAlive = 0;
            foreach (var entity in _competators)
            {
                if (entity.Competator.IsDead == false)
                {
                    if (entity.Side == ECompetatorSide.Ally)
                    {
                        allyAlive++;
                    }
                    else if (entity.Side == ECompetatorSide.Hostile)
                    {
                        hostileAlive++;
                    }
                }
            }

            if (hostileAlive == 0)
            {
                return true;
            }
            else if (allyAlive == 0)
            {
                return true;
            }

            return false;
        }

        #region TurnModifer 
        private IEnumerator AdvanceTurn()
        {
            //Starting new turn 
            ColorfulLogger.LogWithColor("START TURN IN " + _currentSide, Color.blue);

            //Call Enter Turn of the entity, this included running status effect 
            foreach (CombatEntity entity in _activeEntities)
            {
                IEnumerator coroutine = entity.TurnEnter();
                while (coroutine.MoveNext())
                {
                    yield return coroutine.Current;
                }
            }

            List<int> temp = new List<int>();
            //Remove control for character that is not ready 
            for (int i = 0; i < _activeEntities.Count; i++)
            {
                if (!_activeEntities[i].ReadyForControl())
                {
                    Debug.Log(_activeEntities[i].name + "is not ready for control");
                    temp.Add(i);
                    // _activeEntities.RemoveAt(i);

                }
            }

            for (int i = temp.Count - 1; i >= 0; i--)
            {
                _activeEntities.RemoveAt(temp[i]);
            }

            if (_activeEntities.Count > 0)
            {
                _currentEntityIndex = 0;
                yield return SwitchControl(-1, _currentEntityIndex);
            }


            foreach (var v in _activeEntities)
            {
                Debug.Log("Active start is " + v.name); 
            }
            //While loop will keep being called until the turn is end
            while (_activeEntities.Count > 0)
            {
                while (!_activeEntities[_currentEntityIndex].ReadyForControl())
                {
                    _activeEntities.RemoveAt(_currentEntityIndex);
                    _currentEntityIndex = 0;
                    if (_activeEntities.Count <= 0)
                        goto End;
                }
                _state = CombatState.WaitingForAction;
                CombatEntity _entity = _activeEntities[_currentEntityIndex];

                IEnumerator actionCoroutine = _entity.GetAction();

                while (actionCoroutine.MoveNext())
                {

                    if (actionCoroutine.Current != null && actionCoroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)))
                    {
                        ColorfulLogger.LogWithColor("start action", Color.black); 
                        _state = CombatState.Action ;

                        yield return _entity.TakeControlSoftLeave();

                        //Maybe it get overheat or some affect stunt it while controling 
                        if (_entity.ReadyForControl())
                        {
                            //ExecuteAction 
                            yield return ((actionCoroutine.Current as RuntimeEffect).ExecuteRuntimeCoroutine(_entity));

                            yield return ((actionCoroutine.Current as RuntimeEffect).OnExecuteRuntimeDone(_entity));
                        }
                        else
                        {
                            yield return new WaitForSecondsRealtime(2.0f);
                        }
                        //When the action is finish executed (like playing animation), end turn 
 
                        // entity's leave turn 
                        yield return SwitchControl(_currentEntityIndex, _currentEntityIndex);

                        _activeEntities.RemoveAt(_currentEntityIndex);

                        _currentEntityIndex = 0;

                        if (_activeEntities.Count > 0) 
                            yield return SwitchControl(-1, _currentEntityIndex);


                        ColorfulLogger.LogWithColor("almost end of action", Color.black);

                        for (int i = _competators.Count - 1; i >= 0; i--)
                        {
                            if (_competators[i].Competator.IsDead)
                            {
                                if (_competators[i].Side == _currentSide)
                                {
                                    _activeEntities.Remove(_competators[i].Competator);
                                }
                                //No need to remove from the main list if it was player's
                                //_competators.RemoveAt(i);
                            }
                        }
                        ColorfulLogger.LogWithColor("End of action", Color.black);

                    }
                    else
                        yield return actionCoroutine.Current;
                }
                if (EndGameConditionMeet())
                {
                    goto End;
                }
                //If GetAction is null, we wait for end of frame
                yield return new WaitForEndOfFrame();
            }

        End:
            foreach (CombatEntity entity in GetCompetatorsBySide(_currentSide))
            {
                yield return entity.TurnLeave();
            }
            ColorfulLogger.LogWithColor("END TURN IN " + _currentSide, Color.blue);

            //Endding this turn 
        }

        #endregion
        public bool IsCombatEnded()
        {
            if (EndGameConditionMeet())
            {
                SetUpNewCombatEncounter();
            }
            //TODO - Determine how the combat should end  
            return false;
        }

        private void SetUpNewCombatEncounter()
        {
            AssignCompetators(FindObjectOfType<EntityLoader>().LoadData(), ECompetatorSide.Hostile);
            _currentSide = ECompetatorSide.Ally;
            _activeEntities = GetCompetatorsBySide(_currentSide);
        }
        #region GETTER 


        public List<CombatEntity> GetCompetatorsBySide(ECompetatorSide ESide)
        {
            return _competators.Where(v => v.Side == ESide).Select(v => v.Competator).ToList();
        }

        public ECompetatorSide GetCharacterSide(CombatEntity entity)
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


        public IEnumerator SwitchControl(int prev, int next)
        {
            if (prev != -1)
                yield return _activeEntities[prev].LeaveControl();



            if (prev != next)
            {
                for (int i = 0; i < GetCompetatorsBySide(ECompetatorSide.Ally).Count; i++)
                {
                    if (GetCompetatorsBySide(ECompetatorSide.Ally)[i] == _activeEntities[next])
                    {
                        GameSceneSetUPManager.Instance.SelectCharacterCamera(i);
                    }

                }

                yield return _activeEntities[next].TakeControl();
            }

        }

        public IEnumerator ChangeActiveEntityIndex(bool increase = false, bool decrease = false)
        {
            if (_activeEntities.Count > 0)
            {
                int temp = _currentEntityIndex;
                if (increase)
                    _currentEntityIndex = (_currentEntityIndex + 1) % _activeEntities.Count;
                else if (decrease)
                    _currentEntityIndex = _currentEntityIndex == 0 ? (_activeEntities.Count - 1) : (_currentEntityIndex - 1);


                if (temp != _currentEntityIndex)
                {
                    yield return SwitchControl(temp, _currentEntityIndex);
                }
            }
            else
            {
                _currentEntityIndex = 0;
            }

        }

        public void ReceiveKeys(KeyCode key)
        {
            if (_state == CombatState.WaitingForAction)
            {
                if (key == (KeyCode.D))
                {
                    StartCoroutine(ChangeActiveEntityIndex(true, false));
                }
                else if (key == (KeyCode.A))
                {
                    StartCoroutine(ChangeActiveEntityIndex(false, true));
                }
            }
        }

        #endregion

    }
}

