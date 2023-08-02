using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Vanaring_DepaDemo
{
    public class GameSceneSetUPManager : MonoBehaviour
    {
        enum Direction { North, East, South, West };
        public enum CharacterSelect
        {
            PLAYER0,
            PLAYER1,
            PLAYER2
        };
        
        [SerializeField]
        
        public GameObject[] etest
        {
            get { return _etest; }
            set { _etest = value; }
        }
        [Header("This Array should pass the models that want to load on scene.")]
        public GameObject[] _etest;

        public CharacterSelect myCharacterSelect;
        public CinemachineVirtualCamera virtualCamera;
        public GameObject centerMarker;
        public Transform theCenter;
        public Vector3 spawnPosition;
        public Vector3 rotation = Vector3.zero;
        private List<GameObject> playerModels = new List<GameObject>();
        private List<GameObject> enemyModels = new List<GameObject>();

        private Transform lookingTarget;

        public GameObject enemyToGenerate;
        public GameObject playerToGenerate;

        //public GameObject[] etest;
        //public GameObject[] ptest;

        public int numberOfEnemy = 3;
        public int numberOfPlayer = 3;
        public float spacing = 3.0f;
        void Start()
        {
            //lookingTarget = centerMarker.transform;
            virtualCamera.LookAt = theCenter.transform;
            virtualCamera.Follow = centerMarker.transform;
            myCharacterSelect = CharacterSelect.PLAYER0;
            //SpawnObjectAtPosition();
            GenerateModel(virtualCamera, playerModels, enemyModels);
        }
        void Update()
        {
            switch (myCharacterSelect)
            {
                case CharacterSelect.PLAYER0:
                    virtualCamera.Follow = playerModels[0].transform;
                    break;
                case CharacterSelect.PLAYER1:
                    virtualCamera.Follow = playerModels[1].transform;
                    break;
                case CharacterSelect.PLAYER2:
                    virtualCamera.Follow = playerModels[2].transform;
                    break;
            }
        }
        //void SpawnObjectAtPosition()
        //{
        //    if (centerMarker == null)
        //    {
        //        Debug.LogError("Prefab not assigned! Please assign a prefab to instantiate.");
        //        return;
        //    }
        //    GameObject newObject = Instantiate(centerMarker, spawnPosition, Quaternion.identity);

        //    newObject.transform.SetParent(transform);
        //}

        void GenerateModel(CinemachineVirtualCamera virtualCamera, List<GameObject> playerModels, List<GameObject> enemyModels)
        {
            if (enemyToGenerate == null || playerToGenerate == null)
            {
                return;
            }
            float totalWidth_e = (numberOfEnemy - 1) * spacing;
            float totalWidth_p = (numberOfEnemy - 1) * spacing;
            Vector3 startPositionEnemy = transform.position - new Vector3(totalWidth_e * 0.5f, 0f, 0f);
            Vector3 startPositionPlayer = transform.position - new Vector3(totalWidth_p * 0.5f, 0f, 0f);

            for (int i = 0; i < numberOfEnemy; i++)
            {
                Vector3 spawnPosition = startPositionEnemy + new Vector3(i * spacing, 0.5f, 5f);
                GameObject newObject = Instantiate(enemyToGenerate, spawnPosition, Quaternion.identity, transform);
                newObject.transform.rotation = Quaternion.Euler(rotation);
                newObject.name = "Enemy" + i;
                enemyModels.Add(newObject);
                if (i == 0)
                {
                    virtualCamera.LookAt = newObject.transform;
                }
            }

            for (int j = 0; j < numberOfPlayer; j++)
            {
                Vector3 spawnPosition = startPositionPlayer + new Vector3(j * spacing, 0.5f, -3.0f);
                GameObject newObject = Instantiate(playerToGenerate, spawnPosition, Quaternion.identity, transform);
                newObject.name = "Player" + j;
                playerModels.Add(newObject);
                if (j == 0)
                {
                    virtualCamera.Follow = newObject.transform;
                }
            }
        }

        void SetVCLookAt(Transform targetToLookAt)
        {
            if (targetToLookAt == null)
            {
                ColorfulLogger.LogWithColor("No target can be found or forget to assign it.", Color.red);
                return;
            }
            virtualCamera.LookAt = targetToLookAt;
        }

        //private void SetSelectCharacter(CharacterSelect myCharacterSelect, CharacterSelect _cs)
        //{
        //    myCharacterSelect = _cs;
        //}
    }
}
