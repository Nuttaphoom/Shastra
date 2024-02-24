using NaughtyAttributes;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vanaring.Assets.Scripts.DaysSystem.SchoolAction;

namespace Vanaring
{
    [Serializable]
    public class NPCBondingActionCommand : BaseLocationActionCommand
    {
        [Header("NPC Bonding Properties")] 
        [SerializeField]
        private CharacterRelationshipDataSO _characterRelationshipDataSO;

        private NPCBondingScheme _npcBondingScheme;

        private GameObject popUpPanel;


        public NPCBondingActionCommand(NPCBondingActionCommand copied) : base(copied)
        {
            _characterRelationshipDataSO = copied._characterRelationshipDataSO;
            _npcBondingScheme = new NPCBondingScheme(); 
        }

        public override void ExecuteCommand()
        {
            RelationshipHandler _relationshipHandler = PersistentPlayerPersonalDataManager.Instance.RelationshipHandler; 
            int bondLevel = _relationshipHandler.GetCurrentBondLevel(_characterRelationshipDataSO.GetCharacterName) ;

            int currentEXP =  _relationshipHandler.GetRelationshipCapEXP(_characterRelationshipDataSO.GetCharacterName);

            //TODO : Aome need to display relatioship ui here 

            if (popUpPanel != null)
            {
                popUpPanel.GetComponent<RelationshipFriendPanel>().OpenPanel();
                return;
            }
            popUpPanel = MonoBehaviour.Instantiate(PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>("RelationshipFriendPanel"));
            popUpPanel.GetComponent<RelationshipFriendPanel>().InitPanel(_characterRelationshipDataSO);

            if (! _relationshipHandler.IsReadyForHangout(_characterRelationshipDataSO.GetCharacterName))
                popUpPanel.GetComponent <RelationshipFriendPanel>().SetBondButtonListener(PlayBondingScheme);
            else 
                popUpPanel.GetComponent<RelationshipFriendPanel>().SetEventButtonListener(PlayHangoutScheme);


            return; 
        }


        private void PlayBondingScheme()
        {
            popUpPanel.GetComponent<MonoBehaviour>().StartCoroutine(PlayBondingSchemeCoroutine() ) ;
        }
        private void PlayHangoutScheme()
        {
            popUpPanel.GetComponent<MonoBehaviour>().StartCoroutine(PlayHangoutSchemeCoroutine());
        }

        private IEnumerator PlayHangoutSchemeCoroutine()
        {
            Debug.Log("Play Hangout Scheme");
            yield return null; 
        }

        private IEnumerator PlayBondingSchemeCoroutine()
        {
            yield return popUpPanel.GetOrAddComponent<RelationshipFriendPanel>().ClosePanel();

            popUpPanel.SetActive(false); 

           _npcBondingScheme.PlayBondingScheme(_characterRelationshipDataSO); 
        }
    }
}
