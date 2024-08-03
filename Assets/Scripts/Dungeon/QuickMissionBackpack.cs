using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class QuickMissionBackpack : MonoBehaviour, ISceneLoaderWaitForSignal
    {
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
            yield return SetUpBackpack(); 
                
            yield return null; 
        }

        private void Update()
        {
            //Test UseItem functionality  

            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    var caster = DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers[0];
            //    List<RuntimePartyMember> target = new List<RuntimePartyMember>();

            //    target.Add(DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers[0]);



            //    StartCoroutine(UseItem(0, caster, target));
            //}
            //if (Input.GetKeyDown(KeyCode.Y))
            //{
            //    var caster = DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers[0];
            //    List<RuntimePartyMember> target = new List<RuntimePartyMember>();

            //    target.Add(DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers[1]);



            //    StartCoroutine(UseItem(0, caster, target));
            //}
            //if (Input.GetKeyDown(KeyCode.U))
            //{
            //    var caster = DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers[0];
            //    List<RuntimePartyMember> target = new List<RuntimePartyMember>();

            //    target.Add(DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers[2]);



            //    StartCoroutine(UseItem(0, caster, target));
            //}
        }

        //private IEnumerator SelectingTargetCoroutine(out List<RuntimePartyMember> selectedTarget)
        //{
        //    selectedTarget = new List<RuntimePartyMember>(); 

        //    while (selectedTarget.Count == 0)
        //    {
        //        yield return null; 
        //    }
        //}

        public IEnumerator UseItem(int backpackItemIndex)
        {
            BackpackItemData usedItem = _loadedBackpackItem[backpackItemIndex];

            //yield return SelectingTargetCoroutine(); 

            BackpackItemAbilityRuntime runtimeEffect = (usedItem.BackpackItem as CombatUseableItemSO).BackpackActionFactorySO.FactorizeBackpackItemAbilityRuntime(user,target) ; 

            yield return runtimeEffect.UseItemAbilityOutsideCombat() ;

            _backpack.RemoveItemFromBackpack(_loadedBackpackItem[backpackItemIndex].BackpackItem,1) ;

        }

        public IEnumerator SetUpBackpack()
        {
            _backpack = PersistentPlayerPersonalDataManager.Instance.GetBackpack;

            _loadedBackpackItem = _backpack.GetCombatUseableItemSOs() ;

            yield return null; 

        }
    }
}
