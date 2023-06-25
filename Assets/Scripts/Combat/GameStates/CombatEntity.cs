
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;


namespace Vanaring_DepaDemo
{
    public class CombatReferee : MonoBehaviour
    {
        [SerializeField]
        [Header("For testing, we manually assign these competator (including enemy) ")] 
        private List<CombatEntity> _competators = new List<CombatEntity>() ;
        private Queue<CombatEntity> _turnOrder = new Queue<CombatEntity>() ;

       

        private void Awake()
        {
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

            yield return _entity.TurnEnter() ;

            while (! _entity.IsTurnEnd()) {
                IEnumerator actionCoroutine = _entity.GetAction() ;  
                while (actionCoroutine.MoveNext())
                {
                    if (actionCoroutine.Current != null && actionCoroutine.Current is RuntimeEffect)
                    {
                        (actionCoroutine.Current as RuntimeEffect).ExecuteRuntimeCoroutine(_entity); 
                    } 
                }
            }

            yield return _entity.TurnLeave() ;


            //Endding this turn 
        }

        private void ConstructTurnQueue()
        {
            //TODO : Create solid algorithm to dertermine turn order 
            _turnOrder.Clear();
            for (int i =0;  i < _competators.Count; i++)
            {
                _turnOrder.Enqueue(_competators[i]); 
            }


        }

        #endregion

        #region GETTER 
        public bool IsCombatEnded()
        {
            //TODO - Determine how the turn should end  
            return false; 
        }
        #endregion

    }
}

