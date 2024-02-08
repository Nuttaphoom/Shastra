using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "SceneSO", menuName = "ScriptableObject/SceneData/SceneSO")]

    public class SceneDataSO  : ScriptableObject
    {
        
        [SerializeField]
        private GameSceneType _gameSceneType;

        [SerializeField]
        private SceneField _sceneField;

        string uniqueID = "" ;

        public string GetSceneName()
        {
            return _sceneField;
        }

        public GameSceneType GetSceneType()
        {
            return _gameSceneType;
        }

        public string GetSceneID()
        {
            if (uniqueID == null || uniqueID == "") 
                uniqueID = Guid.NewGuid().ToString();
            return uniqueID;
        }

        public enum GameSceneType
        {
            Location, //SceneSelector tool will also load PersistentManagers and Gameplay
            Menu, //SceneSelector tool will also load 
        }
    }
}
 