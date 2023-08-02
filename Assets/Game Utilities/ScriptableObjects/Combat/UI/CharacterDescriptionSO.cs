using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "CharacterDescriptionSO", menuName = "ScriptableObject/UI/CharacterDescriptionSO")]
    public class CharacterDescriptionSO : ScriptableObject
    {
        [SerializeField]
        private string characterName;

        [SerializeField]
        private Image _imageIcon;
    }
}
