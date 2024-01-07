using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.UI;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "TransitionSceneSO", menuName = "ScriptableObject/Scene/TransitionSceneSO")]
    public class TransitionSceneSO : ScriptableObject
    {
        [SerializeField]
        private Sprite loadingScreenSprite;
        [SerializeField]
        private string tipText;
        [SerializeField]
        private Sprite loadingIcon;
        public string TipText => tipText;
        public Sprite ScreenImage => loadingScreenSprite;
    }
}

