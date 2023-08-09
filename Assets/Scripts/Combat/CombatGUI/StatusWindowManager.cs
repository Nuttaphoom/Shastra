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
        private Transform _socketIconLayout;

        [SerializeField]
        private Transform _socketWindowLayout;

        //magic number :D
        Vector3 statusOffset = new Vector3 (0.03f,-0.06f,0.0f);
        List<StatusRuntimeEffect> currentStatusEffects;

        public void Awake()
        {
            _templatePrefab.gameObject.SetActive(false);
            currentStatusEffects = new List<StatusRuntimeEffect>();
        }

        public void InstantiateStatusUI(StatusRuntimeEffect statusEffect)
        {
            StatusSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
            newSocket.transform.parent = _socketIconLayout.transform;
            newSocket.gameObject.transform.position += statusOffset * currentStatusEffects.Count;
            newSocket.transform.localScale = _templatePrefab.transform.localScale;

            currentStatusEffects.Add(statusEffect);

            newSocket.Init(statusEffect, _combatEntity);
            newSocket.gameObject.SetActive(true);
        }
    }
}
