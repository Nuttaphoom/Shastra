using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "SceneSO", menuName = "ScriptableObject/SceneSO")]

    public class SceneDataSO  : ScriptableObject
    {
        [SerializeField]
        private GameSceneType _gameSceneType;

        [SerializeField]
        private SceneField _sceneField;

        public string GetSceneName()
        {
            return _sceneField;
        }

        public GameSceneType GetSceneType()
        {
            return _gameSceneType;
        }

        public enum GameSceneType
        {
            Location, //SceneSelector tool will also load PersistentManagers and Gameplay
            Menu, //SceneSelector tool will also load 
        }
    }
}
 