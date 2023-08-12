using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class StatusWindowManager : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _combatEntity;

        [SerializeField]
        private StatusSocketGUI _templatePrefab;

        [SerializeField]
        private Transform[] _setOfStatusUIPosition;

        //magic number :D
        List<StatusRuntimeEffect> currentStatusEffects;

        public void Awake()
        {
            _templatePrefab.gameObject.SetActive(false);
            currentStatusEffects = new List<StatusRuntimeEffect>();
        }

        public void InstantiateStatusUI(StatusRuntimeEffect statusEffect, int stackcount)
        {
            StatusSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
            int count = currentStatusEffects.Count;
            if (count >= _setOfStatusUIPosition.Length)
            {
                count = 0;
            }
            newSocket.transform.parent = _setOfStatusUIPosition[count].transform;
            newSocket.transform.localScale = _templatePrefab.transform.localScale;

            currentStatusEffects.Add(statusEffect);

            newSocket.Init(statusEffect, _combatEntity);
            newSocket.ChangeBuffStack(statusEffect, stackcount);
            newSocket.gameObject.SetActive(true);
        }
    }
}
