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
    public class ItemWindowGUI : CombatWindowGUI
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
        [SerializeField] private List<GameObject> hidenableObjectList = new List<GameObject>();
        private int currentSelectedIndex = 0;
        public override void OnWindowActive()
        {
            FindObjectOfType<EnemyHUDWindowManager>().DisplayEnemyHUD(CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Hostile));

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
            {
                DisplayArrowIndicator();
                SetUIActive(false);
                _itemSocketTemplate.gameObject.SetActive(false);
                return;
            }
            int tmpItemIndex = 0;
            ClearData();
            _itemSocketTemplate.gameObject.SetActive(true);
            currentSelectedIndex = 0;
            int i = 3 ; 
            foreach (ItemAbilityRuntime item in entity.ItemUser.Items)
            {
                if (entity.ItemUser.ItemsAmount.Count <= tmpItemIndex)
                    throw new Exception("tmpItemIndex in more than ItemAmount.count"); 

                if (entity.ItemUser.ItemsAmount[tmpItemIndex] <= 0)
                    continue;
                ItemSocketGUI newSocket = Instantiate(_itemSocketTemplate, itemTranform.transform) ;
                newSocket.Init(item, entity, entity.ItemUser.ItemsAmount[tmpItemIndex]);
                newSocket.transform.SetAsFirstSibling();
                newSocket.gameObject.SetActive(true);
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
                tmpItemIndex++; 
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
        private void SetUIActive(bool isActive)
        {
            //Debug.Log("Hide");
            foreach (var item in hidenableObjectList)
            {
                item.gameObject.SetActive(isActive);
            }
        }
        private void ScrollToNext()
        {
            //select below index
            int i = 0;
            foreach (ItemSocketGUI item in itemSocketGUIList)
            {
                if (item != null)
                {
                    if (displayingItemIndexList[i] == 6)
                    {
                        displayingItemIndexList[i] = displayingItemIndexList[i] - 1;
                        item.GetComponent<RectTransform>().DOAnchorPos(itemTransformList[displayingItemIndexList[i]].localPosition, 0.1f);
                        break;
                    }
                    if (displayingItemIndexList[i] != 0 && displayingItemIndexList[i] != 6)
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
            DisplayArrowIndicator();
        }
        private void ScrollToPrevious()
        {
            //select above index
            for (int i = itemSocketGUIList.Count - 1; i >= 0; i--)
            {
                if (itemSocketGUIList[i] != null)
                {
                    if (displayingItemIndexList[i] == 6)
                    {
                        displayingItemIndexList[i] = displayingItemIndexList[i] + 1;
                        itemSocketGUIList[i].GetComponent<RectTransform>().DOAnchorPos(itemTransformList[displayingItemIndexList[i]].localPosition, 0.1f);
                        break;
                    }
                    if (displayingItemIndexList[i] != 0 && displayingItemIndexList[i] != 6)
                    {
                        displayingItemIndexList[i] = displayingItemIndexList[i] + 1;
                        itemSocketGUIList[i].GetComponent<RectTransform>().DOAnchorPos(itemTransformList[displayingItemIndexList[i]].localPosition, 0.1f);
                    }
                }
                else
                {
                    Debug.Log("Spell null");
                }
            }

            itemSocketGUIList[currentSelectedIndex].UnHighlightedButton();
            currentSelectedIndex--;
            itemLogText.text = itemSocketGUIList[currentSelectedIndex].GetItemDescription();
            itemSocketGUIList[currentSelectedIndex].HightlightedButton();
            DisplayArrowIndicator();
        }
        private void DisplayArrowIndicator()
        {
            if (currentSelectedIndex < itemSocketGUIList.Count - 1 && itemSocketGUIList.Count > 1 && itemSocketGUIList.Count != 1)
            {
                arrowDown.SetActive(true);
            }
            else
            {
                arrowDown.SetActive(false);
            }
            if (currentSelectedIndex > 0 && itemSocketGUIList.Count > 1 && itemSocketGUIList.Count != 1)
            {
                arrowUp.SetActive(true);
            }
            else
            {
                arrowUp.SetActive(false);
            }
        }
        public override void ReceiveKeysFromWindowManager(InputCode key)
        {
            if (key == InputCode.DeSelect)
            {
                _windowManager.OpenWindow(EWindowGUI.Main);
                _windowManager.PlayPanelAnimation("CloseDescription");
            }
            else if (key == InputCode.Select)
            {
                itemSocketGUIList[currentSelectedIndex].CallButtonCallback();
            }
            else if (key == InputCode.Down)
            {
                if (currentSelectedIndex < itemSocketGUIList.Count - 1 && itemSocketGUIList.Count > 1)
                {
                    ScrollToNext();
                }
            }
            else if (key == InputCode.Up)
            {
                if (currentSelectedIndex > 0 && itemSocketGUIList.Count > 1)
                {
                    ScrollToPrevious();
                }
            }
        }
    }
}
