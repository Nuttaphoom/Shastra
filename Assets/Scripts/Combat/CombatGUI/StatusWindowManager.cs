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

        [SerializeField]
        private Transform _layout;

        //magic number :D
        private List<GameObject> currentStatusObject;

        public void Awake()
        {
            _templatePrefab.gameObject.SetActive(false);
            currentStatusObject = new List<GameObject>();
            if (_combatEntity != null)
            {
                _combatEntity.GetStatusEffectHandler().SubOnStatusVisualEvent(UpdateStatusUI);
            }
        }

        public void InstantiateStatusUI(Dictionary<string, List<StatusRuntimeEffect>> effects)
        {
            foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in effects)
            {
                if (entry.Value != null && entry.Value.Count != 0)
                {
                    StatusSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
                    int count = currentStatusObject.Count;
                    if (count >= _setOfStatusUIPosition.Length)
                    {
                        count = 0;
                    }

                    newSocket.transform.parent = _layout.transform;
                    newSocket.transform.localScale = _setOfStatusUIPosition[count].transform.localScale;

                    currentStatusObject.Add(newSocket.gameObject);

                    newSocket.Init(entry.Value[0], _combatEntity);
                    newSocket.ChangeBuffStack(entry.Value[0], entry.Value.Count);
                    newSocket.gameObject.SetActive(true);
                }
            }
        }
        public void ClearCurrentStatus()
        {
            foreach (GameObject eSocketObj in currentStatusObject)
            {
                Destroy(eSocketObj);
            }
        }

        public void SetCombatEntity(CombatEntity entity)
        {
            _combatEntity = entity;

            _combatEntity.GetStatusEffectHandler().SubOnStatusVisualEvent(UpdateStatusUI);
        }

        public void UpdateStatusUI(Dictionary<string, List<StatusRuntimeEffect>> effects)
        {
            this.ClearCurrentStatus();
            this.InstantiateStatusUI(effects);
        }
    }
}
