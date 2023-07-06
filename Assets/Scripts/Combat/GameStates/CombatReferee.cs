
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        private Queue<CombatEntity> _turnOrder = new Queue<CombatEntity>() ;

        private void Awake()
        {
            instance = this; 

            //Initialize the competators 
            //like loaded data from character sheet 
            foreach (CompetatorDetailStruct competator in _competators)
                 competator.Competator.Init();
            

            StartCoroutine(CustomTick()) ;

        }

        private IEnumerator CustomTick()
        {
            while (!IsCombatEnded())
            {
                if (_turnOrder.Count <= 0)
                {
                    SetupNewRound();
                }                

                yield return AdvanceTurn() ;
            }
        }
        private void SetupNewRound()
        {
            ConstructTurnQueue(); 
        }

        #region TurnModifer 
        private IEnumerator AdvanceTurn()
        {
            //Starting new turn 

            CombatEntity _entity = _turnOrder.Dequeue();
            Debug.Log("new turn : " + _entity.name);

            bool takenAction = false;
            yield return _entity.TurnEnter() ;

            //While loop will keep being called until the turn is end
            while (! takenAction) {
                IEnumerator actionCoroutine = _entity.GetAction() ;  
                while (actionCoroutine.MoveNext())
                {
                    if (actionCoroutine.Current != null && actionCoroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)))
                    {
                        //ExecuteAction 
                        yield return ((actionCoroutine.Current as RuntimeEffect).ExecuteRuntimeCoroutine(_entity));
                        //When the action is finish executed (like playing animation), end turn 
                        takenAction = true; 
                    }  
                }
                //If GetAction is null, we wait for end of frame
                yield return new WaitForEndOfFrame() ; 
            }

        EndTurnFlag:

            Debug.Log("turn end");

            yield return _entity.TurnLeave() ;


            //Endding this turn 
        }

        private void ConstructTurnQueue()
        {
            //TODO : Create solid algorithm to dertermine turn order 
            _turnOrder.Clear();
            for (int i =0;  i < _competators.Count; i++)
            {
                _turnOrder.Enqueue(_competators[i].Competator) ; 
            }


        }

        #endregion

        #region GETTER 
        public bool IsCombatEnded()
        {
            //TODO - Determine how the turn should end  
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
        
        #endregion

    }
}

