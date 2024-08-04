using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class QuickMissionBackpack : MonoBehaviour, ISceneLoaderWaitForSignal, IInputReceiver
    {
        //Easy State machine 
        [SerializeField] private Animator quickItemUIAnim;
        [SerializeField] private QuickItemSocketGUI quickItemSocketTemplate;
        [SerializeField] private GameObject socketTransform;
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
            yield return SetUpBackpack(); 
                
            yield return null; 
        }

         

        public IEnumerator UseItem(int backpackItemIndex)
        {
            Debug.Log("use item at index " + backpackItemIndex) ;  

            _state = EQuckMissionBackpackState.SelectTarget; 

            BackpackItemData usedItem = _loadedBackpackItem[backpackItemIndex];

            //Assign target            
            while (_selectedPartyMember.Count == 0) 
            { 
                yield return null; 
            }

            ////////

            BackpackItemAbilityRuntime runtimeEffect = (usedItem.BackpackItem as CombatUseableItemSO).BackpackActionFactorySO.FactorizeBackpackItemAbilityRuntime(_selectedPartyMember[0], _selectedPartyMember) ; 

            yield return runtimeEffect.UseItemAbilityOutsideCombat() ;

            _backpack.RemoveItemFromBackpack(_loadedBackpackItem[backpackItemIndex].BackpackItem,1) ;

            CloseQuickMissionBackpack(); 
        }

        private void CloseQuickMissionBackpack()
        {
            CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this) ;

            _state = EQuckMissionBackpackState.Closed;

        }

        public IEnumerator SetUpBackpack()
        {
            _backpack = PersistentPlayerPersonalDataManager.Instance.GetBackpack;

            _loadedBackpackItem = _backpack.GetCombatUseableItemSOs() ;

            //Load UI here 
            //foreach (BackpackItemData item in _loadedBackpackItem)
            //{
            //    QuickItemSocketGUI newSocket = Instantiate(quickItemSocketTemplate, socketTransform.transform);
            //    newSocket.Init(item);
            //    newSocket.gameObject.SetActive(true);
            //}
            //quickItemSocketTemplate.gameObject.SetActive(false);

            yield return null; 

        }

        public void ReceiveKeys(InputCode key)
        {

            if (_state == EQuckMissionBackpackState.Closed)
            {
                if (key == InputCode.Item)
                {
                    quickItemUIAnim.Play("OnSelectState");
                    Debug.Log("Enter select item state");
                    _state = EQuckMissionBackpackState.SelectItem;  
                }
            }

            //Highlighted selected item here 
            else if (_state == EQuckMissionBackpackState.SelectItem)
            {
                if (key == InputCode.Left) { 
                    if (_selectedItem > 0)
                        _selectedItem -= 1; 
                } else if (key == InputCode.Right)
                {
                    if (_selectedItem < _loadedBackpackItem.Count - 1  ) 
                        _selectedItem += 1; 
                }else if (key == InputCode.Select)
                {
                    quickItemUIAnim.Play("OnSelectTargetState");
                    StartCoroutine(UseItem( _selectedItem)); 
                }else if (key == InputCode.Item) {
                    quickItemUIAnim.Play("OnOpen");
                    _state = EQuckMissionBackpackState.Closed;
                }
            }


            else if (_state == EQuckMissionBackpackState.SelectTarget)
            {
                if (key == InputCode.Left)
                {
                    if (_selectedTarget > 0)
                        _selectedTarget -= 1;
                }
                else if (key == InputCode.Right)
                {
                    if (_selectedTarget < GetActivePartyMembers().Count - 1)
                        _selectedTarget += 1;
                }
                else if (key == InputCode.Select)
                {
                    _selectedPartyMember.Add(GetActivePartyMembers()[_selectedTarget]); 
                }
            }


        }

        private List<RuntimePartyMember> GetActivePartyMembers()
        {
            return DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers; 
        }
    }
}
