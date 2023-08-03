using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class ItemWindowManager : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _combatEntity;

        [SerializeField]
        private GameObject _templatePrefab;

        [SerializeField]
        private Transform _socketVerticalLayout;

        private List<ItemAbilityRuntime> _itemInventoryData;
        private List<int> _itemInventoryAmount;

        private List<GameObject> _GUIinventoryObject;
        private List<ItemSocketGUI> _GUIinventory;
        // Start is called before the first frame update
        void Start()
        {
            _GUIinventoryObject = new List<GameObject>();
            _GUIinventory = new List<ItemSocketGUI>();
            _itemInventoryData = new List<ItemAbilityRuntime>();
            _itemInventoryAmount = new List<int>();
            UpdateItemSocket(_combatEntity.ItemUser.Items, _combatEntity.ItemUser.ItemsAmount);
        }

        public void ResetGUIinventory()
        {
            _GUIinventory.Clear();
            foreach (GameObject eSocketObj in _GUIinventoryObject)
            {
                Destroy(eSocketObj);
            }
        }
        public void UpdateItemSocket(List<ItemAbilityRuntime> updatedItemdata, List<int> updatedItemAmount)
        {
            ResetGUIinventory();
            _itemInventoryData = updatedItemdata;
            _itemInventoryAmount = updatedItemAmount;

            //Version 1 : Add multiple Item L L L L = x4
            //foreach (ItemAbilityRuntime itemAbility in _itemInventoryData)
            //{
            //    if (ItemIsContained(itemAbility))
            //    {
            //        continue;
            //    }
            //    GameObject newSocketObject = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
            //    newSocketObject.transform.parent = _socketVerticalLayout.transform;
            //    newSocketObject.transform.localScale = _templatePrefab.transform.localScale;
            //    newSocketObject.SetActive(true);
            //    ItemSocketGUI newSocket = newSocketObject.GetComponent<ItemSocketGUI>();
            //    newSocket.Init(itemAbility, _combatEntity);
            //    _GUIinventory.Add(newSocket);
            //    _GUIinventoryObject.Add(newSocketObject);
            //}

            //TODO : Remove Only target item

            //Version 2 : Add multiple Item L 4 = x4
            for (int i = 0 ; i< _itemInventoryData.Count ; i++)
            {
                GameObject newSocketObject = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
                newSocketObject.transform.parent = _socketVerticalLayout.transform;
                newSocketObject.transform.localScale = _templatePrefab.transform.localScale;
                newSocketObject.SetActive(true);
                ItemSocketGUI newSocket = newSocketObject.GetComponent<ItemSocketGUI>();
                newSocket.Init(_itemInventoryData[i], _combatEntity);
                newSocket.SetNumberOfItem(_itemInventoryAmount[i]);
                _GUIinventory.Add(newSocket);
                _GUIinventoryObject.Add(newSocketObject);

            }

            _templatePrefab.gameObject.SetActive(false);

        }

        private bool ItemIsContained(ItemAbilityRuntime itemAbility)
        {
            bool found = false;
            foreach (ItemSocketGUI eSocket in _GUIinventory)
            {
                if (eSocket.IsSameItem(itemAbility.ItemName.ToString()))
                {
                    eSocket.AddItem(1);
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}
