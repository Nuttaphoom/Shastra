using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public NPCBondingActionCommand(NPCBondingActionCommand copied) : base(copied)
        {
            _characterRelationshipDataSO = copied._characterRelationshipDataSO;
            _npcBondingScheme = new NPCBondingScheme(); 
        }

        public override void ExecuteCommand()
        {
            if (PersistentPlayerPersonalDataManager.Instance.RelationshipHandler.IsReadyForHangout(_characterRelationshipDataSO.GetCharacterName)  ) {
                PlayHangoutScheme();         
            }else {
                PlayBondingScheme(); 
            }

            return; 
        }

        private void PlayHangoutScheme()
        {
            Debug.Log("Play Hangout Scheme");
        }

        private void PlayBondingScheme()
        {
            Debug.Log("Bonding Scheme"); 
   

            _npcBondingScheme.PlayingBondingScheme(_characterRelationshipDataSO); 
        }
    }
}
