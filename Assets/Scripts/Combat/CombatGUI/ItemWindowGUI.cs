using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Vanaring
{
    public class ItemWindowGUI : WindowGUI
    {
        [SerializeField]
        private ItemSocketGUI _itemSocketTemplate;
        [SerializeField] Transform[] itemTransformList;
        private List<ItemSocketGUI> itemSocketGUIList = new List<ItemSocketGUI>();
        private List<int> displayingItemIndexList = new List<int>();
        [SerializeField] private TextMeshProUGUI itemLogText;
        [SerializeField] private GameObject itemTranform;
        [SerializeField] private GameObject arrowUp;
        [SerializeField] private GameObject arrowDown;
        private int itemIndexFocusUpMin = 0;
        private int itemIndexFocusUpMax = 3;
        private int itemIndexFocusDownMin = 0;
        private int itemIndexFocusDownMax = 2;
        private int currentSelectedIndex = 0;

        public override void OnWindowActive()
        {
            
        }

        public override void OnWindowDeActive()
        {
            

        }

        public override void ClearData()
        {
            for (int index = itemSocketGUIList.Count - 1; index >= 0; index--)
            {
                Destroy(itemSocketGUIList[index].gameObject);
                itemSocketGUIList.RemoveAt(index);
            }
            for (int index = displayingItemIndexList.Count - 1; index >= 0; index--)
            {
                displayingItemIndexList[index] = 0;
                displayingItemIndexList.RemoveAt(index);
            }
            displayingItemIndexList.Clear();
            itemSocketGUIList.Clear();
        }
        public override void LoadWindowData(CombatEntity entity)
        {
            if (entity.ItemUser.Items.Count <= 0)
                return;  
             
            int tmpItemIndex = 0;
            ClearData();
            _itemSocketTemplate.gameObject.SetActive(true);
            itemIndexFocusUpMin = 0;
            itemIndexFocusUpMax = 3;
            itemIndexFocusDownMin = 0;
            itemIndexFocusDownMax = 2;
            currentSelectedIndex = 0;
            int i = 3 ; 
            foreach (ItemAbilityRuntime item in entity.ItemUser.Items)
            {
                if (entity.ItemUser.ItemsAmount[tmpItemIndex] <= 0)
                    continue;
                ItemSocketGUI newSocket = Instantiate(_itemSocketTemplate, itemTranform.transform) ;
                newSocket.Init(item, entity, entity.ItemUser.ItemsAmount[tmpItemIndex]);
                newSocket.transform.SetAsFirstSibling();
                itemSocketGUIList.Add(newSocket);
                if (itemSocketGUIList.Count > 3)
                {
                    newSocket.transform.position = itemTransformList[6].transform.position;
                    displayingItemIndexList.Add(6);
                }
                else
                {
                    newSocket.transform.position = itemTransformList[i].transform.position;
                    displayingItemIndexList.Add(i);
                }
                newSocket.UnHighlightedButton();
                i++;
            }

            if (entity.ItemUser.Items.Count == 0)
            {
                DisplayArrowIndicator();
                itemLogText.text = "There's no item can be use.";
                _itemSocketTemplate.gameObject.SetActive(false);
                return;
            }
            _itemSocketTemplate.gameObject.SetActive(false);
            itemLogText.text = itemSocketGUIList[currentSelectedIndex].GetItemDescription();
            itemSocketGUIList[0].HightlightedButton();
        }
        private void ScrollToNext()
        {
            //select below index
            int i = 0;
            foreach (ItemSocketGUI item in itemSocketGUIList)
            {
                if (item != null)
                {
                    if (displayingItemIndexList[i] > 0 && i >= itemIndexFocusUpMin && i <= itemIndexFocusUpMax)
                    {
                        displayingItemIndexList[i] = displayingItemIndexList[i] - 1;
                        item.GetComponent<RectTransform>().DOAnchorPos(itemTransformList[displayingItemIndexList[i]].localPosition, 0.1f);
                    }
                }
                else
                {
                    Debug.Log("Spell null");
                }
                i++;
            }
            itemSocketGUIList[currentSelectedIndex].UnHighlightedButton();
            currentSelectedIndex++;
            itemLogText.text = itemSocketGUIList[currentSelectedIndex].GetItemDescription();
            itemSocketGUIList[currentSelectedIndex].HightlightedButton();
            UpdateIndexFocusOnInputCall();
            DisplayArrowIndicator();
        }

        private void ScrollToPrevious()
        {
            //select above index
            int i = 0;
            foreach (ItemSocketGUI spell in itemSocketGUIList)
            {
                if (spell != null)
                {
                    if (displayingItemIndexList[i] <= itemTransformList.Length - 1 && i >= itemIndexFocusDownMin && i <= itemIndexFocusDownMax)
                    {
                        displayingItemIndexList[i] = displayingItemIndexList[i] + 1;
                        spell.GetComponent<RectTransform>().DOAnchorPos(itemTransformList[displayingItemIndexList[i]].localPosition, 0.1f);
                    }
                }
                else
                {
                    Debug.Log("Spell null");
                }
                i++;
            }
            itemSocketGUIList[currentSelectedIndex].UnHighlightedButton();
            currentSelectedIndex--;
            itemLogText.text = itemSocketGUIList[currentSelectedIndex].GetItemDescription();
            itemSocketGUIList[currentSelectedIndex].HightlightedButton();
            UpdateIndexFocusOnInputCall();
            DisplayArrowIndicator();
        }
        private void DisplayArrowIndicator()
        {
            if (currentSelectedIndex < itemSocketGUIList.Count - 1 && itemSocketGUIList.Count > 1)
            {
                arrowDown.SetActive(true);
            }
            else
            {
                arrowDown.SetActive(false);
            }
            if (currentSelectedIndex > 0 && itemSocketGUIList.Count > 1)
            {
                arrowUp.SetActive(true);
            }
            else
            {
                arrowUp.SetActive(false);
            }
        }

        private void UpdateIndexFocusOnInputCall()
        {
            switch (currentSelectedIndex)
            {
                case 0:
                    itemIndexFocusUpMin = 0;
                    itemIndexFocusUpMax = 3;
                    itemIndexFocusDownMin = 0;
                    itemIndexFocusDownMax = 2;
                    break;
                case 1:
                    itemIndexFocusUpMin = 0;
                    itemIndexFocusUpMax = 4;
                    itemIndexFocusDownMin = 0;
                    itemIndexFocusDownMax = 3;
                    break;
                case 2:
                    itemIndexFocusUpMin = 0;
                    itemIndexFocusUpMax = 4;
                    itemIndexFocusDownMin = 0;
                    itemIndexFocusDownMax = 4;
                    break;
                case 3:
                    itemIndexFocusUpMin = 1;
                    itemIndexFocusUpMax = 4;
                    itemIndexFocusDownMin = 0;
                    itemIndexFocusDownMax = 4;
                    break;
                case 4:
                    itemIndexFocusUpMin = 2;
                    itemIndexFocusUpMax = 4;
                    itemIndexFocusDownMin = 1;
                    itemIndexFocusDownMax = 4;
                    break;

            }
            //Debug.Log("FocusUpMin: " + spellIndexFocusUpMin + " FocusUpMax: " + spellIndexFocusUpMax);
            //Debug.Log("FocusDownMin: " + spellIndexFocusDownMin + " FocusDownMax: " + spellIndexFocusDownMax);
        }

        public override void ReceiveKeysFromWindowManager(KeyCode key)
        {
            if (key == KeyCode.Q)
            {
                _windowManager.OpenWindow(EWindowGUI.Main);
            }
            else if (key == KeyCode.Space)
            {
                itemSocketGUIList[currentSelectedIndex].CallButtonCallback();
            }
            else if (key == KeyCode.S)
            {
                if (currentSelectedIndex < itemSocketGUIList.Count - 1 && itemSocketGUIList.Count > 1)
                {
                    ScrollToNext();
                }
            }
            else if (key == KeyCode.W)
            {
                if (currentSelectedIndex > 0 && itemSocketGUIList.Count > 1)
                {
                    ScrollToPrevious();
                }
            }
        }
    }
}
