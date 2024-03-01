using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "CombatCharacterSheet", menuName = "ScriptableObject/Character/CombatCharacterSheet")]

    public class CombatCharacterSheetSO : CharacterSheetSO 
    {

        [Header("Peak HP of this entity")]
        [SerializeField]
        private int _HP;

        [Header("Default ATK of this entity")]
        [SerializeField]
        private int _ATK;

        [Header("Default MP of this entity")]
        [SerializeField]
        private int _MP; 

        [Header("Ailment Resistant")]
        [SerializeField]
        private  AilmentResistantDataInfo _ailmentResistantDataInfo;

        [Header("Character Sprite")]
        [SerializeField]
        private  Sprite _characterIconGUI;

        public AilmentResistantDataInfo ResistantData => _ailmentResistantDataInfo;

        public Sprite GetCharacterIcon => _characterIconGUI;
        public int GetHP => _HP;
        public int GetATK => _ATK;

        public int GetMP => _MP;
    }
}
