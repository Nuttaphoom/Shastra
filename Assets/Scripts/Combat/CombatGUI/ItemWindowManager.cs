using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class ItemWindowManager : HierarchyUIWindow
    {
        [SerializeField]
        private CombatEntity _combatEntity;

        [SerializeField]
        private GameObject _templatePrefab;

        [SerializeField]
        private Transform _socketParent;

        [SerializeField]
        private Transform[] _itemSocketGUITransformPos;

        private List<ItemAbilityRuntime> _itemInventoryData;
        private List<int> _itemInventoryAmount;

        private List<GameObject> _GUIinventoryObject;
        private List<ItemSocketGUI> _GUIinventory;

        private CombatGraphicalHandler _graphicalHandler;
        

        // Start is called before the first frame update
        void Awake()
        {
            _GUIinventoryObject = new List<GameObject>();
            _GUIinventory = new List<ItemSocketGUI>();
            _itemInventoryData = new List<ItemAbilityRuntime>();
            _itemInventoryAmount = new List<int>();
            
            _templatePrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
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
            Debug.Log("Update");
            ResetGUIinventory();
            _itemInventoryData = updatedItemdata;
            _itemInventoryAmount = updatedItemAmount;

            #region
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
            #endregion
            //Version 2 : Add multiple Item L 4 = x4
            for (int i = 0; i < _itemInventoryData.Count; i++)
            {
                GameObject newSocketObject = Instantiate(_templatePrefab, _socketParent.transform);
                newSocketObject.transform.position = _itemSocketGUITransformPos[i].transform.position;
                newSocketObject.transform.localScale = _templatePrefab.transform.localScale;
                newSocketObject.SetActive(true);
                newSocketObject.transform.SetAsFirstSibling();

                ItemSocketGUI newSocket = newSocketObject.GetComponent<ItemSocketGUI>();
                newSocket.Init(_itemInventoryData[i], _combatEntity);
                newSocket.SetNumberOfItem(_itemInventoryAmount[i]);

                _GUIinventory.Add(newSocket);
                _GUIinventoryObject.Add(newSocketObject);
            }
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
        

        private void LoadLowerSocketItem()
        {
            //if (endIndex < _combatEntity.SpellCaster.SpellAbilities.Count - 1)
            //{
            //    ResetGUIinventory();
            //    startIndex++;
            //    endIndex++;
            //    LoadSpellSocketGUI(startIndex, endIndex);
            //}
        }

        private void LoadUpperSocketItem()
        {
            //if (startIndex > 0)
            //{
            //    ResetGUIinventory();
            //    startIndex--;
            //    endIndex--;
            //    LoadSpellSocketGUI(startIndex, endIndex);
            //}
        }

        //public void TakeInputControl()
        //{
        //    CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
        //}

        //public void ReleaseInputControl()
        //{
        //    CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);
        //}

        public void ReceiveKeys(KeyCode key)
        {
            if (key == KeyCode.W)
            {
                //LoadUpperSocketItem();
            }
            else if (key == KeyCode.S)
            {
                //LoadLowerSocketItem();
            }
             
        }

        public override void OnWindowDisplay(CombatGraphicalHandler graophicalHandler)
        {
            SetGraphicMenuActive(true);
        }

        public override void OnWindowOverlayed()
        {
            SetGraphicMenuActive(false); 
        }
    }
}
