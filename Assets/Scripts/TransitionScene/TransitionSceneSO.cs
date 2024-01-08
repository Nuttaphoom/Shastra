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
        private TransitionObject transitionObjPrefab;

        //[SerializeField]
        //private Sprite loadingScreenSprite;
        
        //[SerializeField]
        //private Sprite loadingIcon;
        

        [Header("Tip information")]
        [SerializeField]
        private string tipText;

        [Header("Time setup")]
        [SerializeField]
        private float plusDelayTime;
        [SerializeField]
        private float speedMultiply = 1;
        public string TipText => tipText;
        //public Sprite ScreenImage => loadingScreenSprite;
    }
}

