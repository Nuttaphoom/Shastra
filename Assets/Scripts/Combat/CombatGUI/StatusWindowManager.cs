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
        private Transform _socketVerticalLayout;

        public void Awake()
        {
            _templatePrefab.gameObject.SetActive(false);
        }

        public void InstantiateStatusUI(StatusRuntimeEffect statusEffect)
        {
            StatusSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
            newSocket.transform.parent = _socketVerticalLayout.transform;
            newSocket.transform.localScale = _templatePrefab.transform.localScale;

            newSocket.Init(statusEffect, _combatEntity);
            newSocket.gameObject.SetActive(true);
        }
    }
}
