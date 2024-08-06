using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public class QuickMissionBackpack : MonoBehaviour, ISceneLoaderWaitForSignal, IInputReceiver
    {
        //Easy State machine 
        [SerializeField] private Animator quickItemUIAnim;
        [SerializeField] private QuickItemSocketGUI quickItemSocketTemplate;
        [SerializeField] private GameObject socketTransform;
        private List<QuickItemSocketGUI> quickItemSocketList = new List<QuickItemSocketGUI>();
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private Animator floatingBox;
        [SerializeField] private GameObject onSelectTargetFader;
        private void Awake()
        {
            CentralInputReceiver.Instance.AddInputReceiverIntoStack(this); 
        }
        private enum EQuckMissionBackpackState
        {
            Closed, 
            SelectItem, 
            SelectTarget 
        }

        private EQuckMissionBackpackState _state = EQuckMissionBackpackState.Closed ;
        //Target selection parameters 
        private int _selectedTarget = 0;
        private int _selectedItem = 0;

        private List<RuntimePartyMember> _selectedPartyMember = new List<RuntimePartyMember>(); 

        //////

        private Backpack _backpack;

        /// <summary>
        /// is the item staying in backpack when mission is loaded
        /// </summary>
        private List<BackpackItemData> _loadedBackpackItem; 
        
        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            quickItemUIAnim.Play("OnOpen");
            onSelectTargetFader.SetActive(false);
            yield return SetUpBackpack(); 
                
            yield return null; 
        }

        public IEnumerator UseItem(int backpackItemIndex)
        {
            Debug.Log("use item at index " + backpackItemIndex) ;

            SetQuickBackpackStaet(EQuckMissionBackpackState.SelectTarget); 
            
            BackpackItemData usedItem = _loadedBackpackItem[backpackItemIndex];

            //Assign target            
            while (_selectedPartyMember.Count == 0) 
            {
                if (_state == EQuckMissionBackpackState.Closed)
                    goto End; 
                yield return null; 
            }

            ////////

            BackpackItemAbilityRuntime runtimeEffect = (usedItem.BackpackItem as CombatUseableItemSO).BackpackActionFactorySO.FactorizeBackpackItemAbilityRuntime(_selectedPartyMember[0], _selectedPartyMember) ; 

            yield return runtimeEffect.UseItemAbilityOutsideCombat() ;

            _backpack.RemoveItemFromBackpack(_loadedBackpackItem[backpackItemIndex].BackpackItem,1) ;

            UpdateItemAmount();

            CloseQuickMissionBackpack();

        End:
            yield return null; 
        }

        private void SetQuickBackpackStaet(EQuckMissionBackpackState nextState)
        {

            _state = nextState;
            switch (_state)
            {
                case EQuckMissionBackpackState.Closed:
                    quickItemUIAnim.Play("OnOpen");
                    
                    break;
                case EQuckMissionBackpackState.SelectItem:
                    break;
                case EQuckMissionBackpackState.SelectTarget:
                    break;
            }
        }

        private void CloseQuickMissionBackpack()
        {
            Debug.Log("close item");
            foreach (QuickItemSocketGUI socket in quickItemSocketList)
            {
                socket.OnDeSelect();
            }
            onSelectTargetFader.SetActive(false);
            //CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this) ;
            PersistentButtonSelector.Instance.RestoreCaptureButton(); 
            quickItemUIAnim.Play("OnDeSelectState");
            _selectedPartyMember.Clear(); 

            SetQuickBackpackStaet(EQuckMissionBackpackState.Closed);

        }

        public IEnumerator SetUpBackpack()
        {
            _backpack = PersistentPlayerPersonalDataManager.Instance.GetBackpack;

            _loadedBackpackItem = _backpack.GetCombatUseableItemSOs() ;

            //Load UI here 
            UpdateItemAmount();
            foreach (QuickItemSocketGUI socket in quickItemSocketList)
            {
                socket.OnDeSelect();
            }

            yield return null; 

        }
        public void UpdateItemAmount()
        {
            _selectedTarget = 0;
            _selectedItem = 0;
            foreach (QuickItemSocketGUI gui in quickItemSocketList)
            {
                Destroy(gui.gameObject);
            }
            quickItemSocketList.Clear();

            Debug.Log(PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCombatUseableItemSOs().Count);

            foreach (BackpackItemData item in PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCombatUseableItemSOs())
            {
                QuickItemSocketGUI newSocket = Instantiate(quickItemSocketTemplate, socketTransform.transform);
                newSocket.Init(item);
                newSocket.gameObject.SetActive(true);
                newSocket.OnDeSelect();
                quickItemSocketList.Add(newSocket);
            }
            
            quickItemSocketTemplate.gameObject.SetActive(false);
        }

        public void DisplayItemDetail(BackpackItemData data)
        {
            foreach (QuickItemSocketGUI socket in quickItemSocketList)
            {
                socket.OnDeSelect();
            }
            quickItemSocketList[_selectedItem].OnSelectIndex(_selectedItem, quickItemSocketList.Count);
            itemNameText.text = data.BackpackItem.GetRewardData().RewardName;
            itemDescriptionText.text = data.BackpackItem.GetRewardData().RewardDescription;
        }

        public void ReceiveKeys(InputCode key)
        {

            if (_state == EQuckMissionBackpackState.Closed)
            {
                //Debug.Log("here"); 

                if (key == InputCode.Item && PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCombatUseableItemSOs().Count > 0)
                {
                    PersistentButtonSelector.Instance.CaptureCurrentSelectedButton() ;

                    PersistentButtonSelector.Instance.DeSelectedButton() ;

                    quickItemUIAnim.Play("OnSelectState");
                    Debug.Log("Enter select item state");
                    quickItemSocketList[0].OnSelectIndex(_selectedItem, quickItemSocketList.Count);
                    _state = EQuckMissionBackpackState.SelectItem;  
                }
            }

            //Highlighted selected item here 
            else if (_state == EQuckMissionBackpackState.SelectItem)
            {
                if (key == InputCode.Left) { 
                    if (_selectedItem > 0)
                    {
                        _selectedItem -= 1;
                        DisplayItemDetail(_loadedBackpackItem[_selectedItem]);
                    }                
                } 
                else if (key == InputCode.Right)
                {
                    if (_selectedItem < _loadedBackpackItem.Count - 1  )
                    {
                        _selectedItem += 1;
                        DisplayItemDetail(_loadedBackpackItem[_selectedItem]);
                    }
                }
                else if (key == InputCode.Select)
                {
                    quickItemUIAnim.Play("OnSelectTargetState");
                    floatingBox.Play("OnSelectTargetState");
                    onSelectTargetFader.SetActive(true);
                    FindAnyObjectByType<MissionPartyHUDManager>().OnSelectHighlight();
                    StartCoroutine(UseItem( _selectedItem));
                    FindAnyObjectByType<MissionPartyHUDManager>().OnSelectTargetSocketHighlight(0);
                }
                else if (key == InputCode.DeSelect) 
                {
                    CloseQuickMissionBackpack(); 
                }
            }


            else if (_state == EQuckMissionBackpackState.SelectTarget)
            {
                if (key == InputCode.Left)
                {
                    if (_selectedTarget > 0)
                    {
                        _selectedTarget -= 1;
                        FindAnyObjectByType<MissionPartyHUDManager>().OnSelectTargetSocketHighlight(_selectedTarget);
                    }
                }
                else if (key == InputCode.Right)
                {
                    if (_selectedTarget < GetActivePartyMembers().Count - 1)
                    {
                        _selectedTarget += 1;
                        FindAnyObjectByType<MissionPartyHUDManager>().OnSelectTargetSocketHighlight(_selectedTarget);
                    }
                }
                else if (key == InputCode.Select)
                {
                    _selectedPartyMember.Add(GetActivePartyMembers()[_selectedTarget]);
                    quickItemSocketList[_selectedItem].UseItemUpdate();
                    onSelectTargetFader.SetActive(false);
                    foreach (QuickItemSocketGUI socket in quickItemSocketList)
                    {
                        socket.OnDeSelect();
                    }
                    FindAnyObjectByType<MissionPartyHUDManager>().OnDeSelectHighlight();
                    
                }
                else if (key == InputCode.DeSelect)
                {
                    CloseQuickMissionBackpack();
                }
            }


        }

        private List<RuntimePartyMember> GetActivePartyMembers()
        {
            return DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers; 
        }
    }
}