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
        private SceneDataSO _sceneData;

        [SerializeField]
        private List<Sprite> screenImageList = new List<Sprite>();
        [SerializeField]
        private List<string> tipTexts = new List<string>();
        [SerializeField]
        private List<Sprite> loadingIconList = new List<Sprite>();
        public SceneDataSO SceneData => _sceneData;
        public List<string> TipText => tipTexts;
        public List<Sprite> ScreenImage => screenImageList;
    }
}

