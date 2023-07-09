
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


namespace Vanaring_DepaDemo
{

    public enum ECompetatorSide
    {
        Ally, 
        Hostile 
    }


    public class CombatReferee : MonoBehaviour
    {
        public static CombatReferee instance = null ; 

        [Serializable] 
        private struct CompetatorDetailStruct
        {
            [SerializeField]
            private ECompetatorSide _side ;
            [SerializeField] 
            private CombatEntity _entity ;

            public ECompetatorSide Side => _side;  
            public CombatEntity Competator => _entity;
        }

        [SerializeField]
        [Header("For testing, we manually assign these competator (including enemy) ")]

        List<CompetatorDetailStruct> _competators; 

        private ECompetatorSide _currentSide = ECompetatorSide.Hostile ; // Assign this to the opposite of the actual turn we want to start with 
        private List<CombatEntity> _activeEntities = new List<CombatEntity> ();

        private int _currentEntityIndex = 0; 

        private void Awake()
        {
            instance = this; 

            //Initialize the competators 
            //like loaded data from character sheet 
            foreach (CompetatorDetailStruct competator in _competators)
                 competator.Competator.Init();
            
            StartCoroutine(CustomTick()) ;
        }

        private void Update()
        {
            //TODO : Centralize the input 
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(ChangeActiveEntityIndex(-1, true, false) ) ;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(ChangeActiveEntityIndex(-1, false, true));
            }
        }

        private IEnumerator CustomTick()
        {
            while (!IsCombatEnded())
            {
                if (_activeEntities.Count <= 0)
                {
                    SetupNewRound();
                }                

                yield return AdvanceTurn() ;
            }
        }
        private void SetupNewRound()
        {
            _currentSide = (ECompetatorSide)(((int)_currentSide + 1) % 2)   ;
            _activeEntities = GetCompetatorsBySide(_currentSide)            ;
        }

        #region TurnModifer 
        private IEnumerator AdvanceTurn()
        {
            //Starting new turn 
            Debug.Log("START TURN IN " + _currentSide);
            //Call Enter Turn of the entity, this included running status effect 
            foreach (CombatEntity entity in _activeEntities)
            {
                IEnumerator coroutine =  entity.TurnEnter() ; 
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
                yield return _activeEntities[_currentEntityIndex].TakeControl();
            }
            //While loop will keep being called until the turn is end
            while (_activeEntities.Count > 0) {
                CombatEntity _entity = _activeEntities[_currentEntityIndex] ;

                IEnumerator actionCoroutine = _entity.GetAction() ;

                while (actionCoroutine.MoveNext())
                {
                    if (actionCoroutine.Current != null && actionCoroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)))
                    {
                        //ExecuteAction 
                        yield return ((actionCoroutine.Current as RuntimeEffect).ExecuteRuntimeCoroutine(_entity));
                        //When the action is finish executed (like playing animation), end turn 

                        if (_activeEntities.Count > 1)
                            yield return SwitchControl(_currentEntityIndex, (_currentEntityIndex + 1) % _activeEntities.Count);
                        else
                            yield return SwitchControl(_currentEntityIndex, _currentEntityIndex) ;

                        _activeEntities.RemoveAt(_currentEntityIndex);
                    }
                    else
                        yield return actionCoroutine.Current;  
                }
                //If GetAction is null, we wait for end of frame
                yield return new WaitForEndOfFrame() ; 
            }


            foreach (CombatEntity entity in _activeEntities)
            {
                yield return entity.TurnLeave() ;
            }

            //Endding this turn 
        }

        #endregion

        #region GETTER 
        public bool IsCombatEnded()
        {
            //TODO - Determine how the combat should end  
            return false; 
        }

        public List<CombatEntity> GetCompetatorsBySide(ECompetatorSide ESide)
        {
            return _competators.Where(v => v.Side == ESide).Select(v => v.Competator).ToList(); 
        }

        public ECompetatorSide GetCharacterSide(CombatEntity entity)
        {
            foreach (var competator in _competators) {
                if (competator.Competator == entity ) {
                    return competator.Side; 
                }
            }

            throw new Exception("Given entity is not registered in CombatReferee"); 
        }
        

        public IEnumerator SwitchControl(int prev, int next)
        {
            yield return _activeEntities[prev].LeaveControl();
            if (prev != next) 
                yield return _activeEntities[next].TakeControl();
        }
        public IEnumerator ChangeActiveEntityIndex(int index = -1, bool increase = false, bool decrease = false)
        {
            if (_activeEntities.Count > 0)
            {
                int temp = _currentEntityIndex ;
                if (index != -1)
                {
                    _currentEntityIndex = index;
                }
                else
                {
                    if (increase)
                        _currentEntityIndex = (_currentEntityIndex + 1) % _activeEntities.Count;
                    else if (decrease)
                           _currentEntityIndex = _currentEntityIndex == 0 ? (_activeEntities.Count -1) : (_currentEntityIndex - 1) ;
                }

                if (temp != _currentEntityIndex )
                {
                    yield return SwitchControl(temp,_currentEntityIndex); 
                }
            }else
            {
                _currentEntityIndex = 0;
            }

        }
        
        #endregion

    }
}

