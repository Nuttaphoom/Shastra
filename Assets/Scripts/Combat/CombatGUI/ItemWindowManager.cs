using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class ItemWindowManager : HierarchyUIWindow, IInputReceiver
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

        private int _currentIndex = 0;
        private List<ItemSocketGUI> _itemSocketGUI = new List<ItemSocketGUI>(); 

        // Start is called before the first frame update
        void Awake()
        {
            _GUIinventoryObject = new List<GameObject>();
            _GUIinventory = new List<ItemSocketGUI>();
            _itemInventoryData = new List<ItemAbilityRuntime>();
            _itemInventoryAmount = new List<int>();
            
            _templatePrefab.gameObject.SetActive(false);
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

            _itemSocketGUI = new List<ItemSocketGUI>(); 
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
                _itemSocketGUI.Add(newSocket); 
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

        

        public void ReceiveKeys(KeyCode key)
        {
            _itemSocketGUI[_currentIndex].UnHighlightedButton();

            if (key == KeyCode.W)
            {
                _currentIndex -= 1;
                if (_currentIndex < 0)
                {
                    LoadUpperSocketItem();

                    _currentIndex = 0;
                }
            }
            else if (key == KeyCode.S)
            {
                _currentIndex += 1;

                if (_currentIndex > _itemSocketGUI.Count - 1)
                {
                    LoadLowerSocketItem();
                    _currentIndex -= 1;

                }
            }
            else if (key == KeyCode.Space)
            {
                _itemSocketGUI[_currentIndex].CallButtonCallback();
            }
            else if (key == KeyCode.Q)
            {
                this._combatGraphicalHandler.DisplayMainMenu();
            }

            _itemSocketGUI[_currentIndex].HightlightedButton();


        }

        public override void OnWindowDisplay(CombatGraphicalHandler graophicalHandler)
        {
            for (int i = 0; i < _itemSocketGUI.Count; i++)
                _itemSocketGUI[i].UnHighlightedButton();

            _currentIndex = 0; 
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
            _itemSocketGUI[_currentIndex].HightlightedButton();

            _combatGraphicalHandler = graophicalHandler; 
            SetGraphicMenuActive(true);
        }

        public override void OnWindowOverlayed()
        {
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this) ;
            for (int i = 0;  i < _itemSocketGUI.Count; i++) 
                _itemSocketGUI[i].UnHighlightedButton();

            SetGraphicMenuActive(false); 
        }
    }
}
