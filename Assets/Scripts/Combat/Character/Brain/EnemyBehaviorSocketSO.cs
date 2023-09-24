using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


namespace Vanaring  {

    [CreateAssetMenu(fileName = "EnemyBehaviorSocketSO", menuName = "ScriptableObject/BotBehavior/BehaviorSocket/EnemyBehaviorSocketSO")]
    //Once behavior socket contains multiple behavior
    public class EnemyBehaviorSocketSO : ScriptableObject
    {
        [SerializeField]
        private List<BotBehaviorSO> _botBehaviorSO;

        public IEnumerator GetBehaviorEffect(int index)
        {
            IEnumerator coroutine =  _botBehaviorSO[index].GetBehaviorEffect();

            while (coroutine.MoveNext() ) {
                if ( coroutine.Current != null &&  coroutine.Current is (BotBehaviorSO.ActionData))
                {
                    yield return (BotBehaviorSO.ActionData) coroutine.Current ;
                }
            }
        }

        #region GETTER
        public int GetBehaviorSize => _botBehaviorSO.Count;
        public EnergyModifierData GetEnergyCost(int index) => _botBehaviorSO[index].ActionEnergyCost ;  
        #endregion
    }
}