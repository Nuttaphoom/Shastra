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
        private string transitionName = null;

        [Header("Information")]
        [SerializeField]
        private TransitionObject transitionObjPrefab;

        [Header("Detail")]
        [SerializeField]
        private string tipText;

        [Header("Time setup")]
        [SerializeField]
        private float loadingDelayTime;
        private float speedMultiply = 1;
        public string TipText => tipText;
        //public Sprite ScreenImage => loadingScreenSprite;
        public TransitionObject GetTransitionObject => transitionObjPrefab;
        public float GetDelayLoadingTime => loadingDelayTime;
    }
}

