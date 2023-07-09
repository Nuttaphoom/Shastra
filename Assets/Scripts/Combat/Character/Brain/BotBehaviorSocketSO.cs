using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


namespace Vanaring_DepaDemo {

    [CreateAssetMenu(fileName = "BotBehaviorSocketSO", menuName = "ScriptableObject/BotBehavior/BehaviorSocket/Socket")]
    //Once behavior socket contains multiple behavior
    public class BotBehaviorSocketSO : ScriptableObject
    {
        [SerializeField]
        private List<BotBehaviorSO> _botBehaviorSO;

        public IEnumerator GetBehaviorEffect(int index)
        {
            IEnumerator coroutine =  _botBehaviorSO[index].GetBehaviorEffect();
            while (coroutine.MoveNext() ) {

                if ( coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffectFactorySO)))
                {
                    yield return coroutine.Current as RuntimeEffectFactorySO ; 
                }
            }
             
        }

        #region GETTER
        public int GetBehaviorSize => _botBehaviorSO.Count;
        #endregion
    }
}