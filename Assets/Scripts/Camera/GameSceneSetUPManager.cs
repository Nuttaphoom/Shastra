using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Vanaring_DepaDemo
{
    public class GameSceneSetUPManager : MonoBehaviour
    {
        public enum CameraOffset { LEFT, MIDDLE, RIGHT };
        
        [Header("This Array pass any number of models want to load on the scene.")]
        public List<GameObject> _enemyModelSetupList = new List<GameObject>();
        public List<GameObject> _characterModelSetupList = new List<GameObject>();

        private Vector3 rotation = new Vector3(0, 180.0f, 0);
        private bool IsTargetMode = false;

        //private CameraOffset myCameraOffset;
        [Header("Camera Type")]
        public CinemachineVirtualCamera RvirtualCamera;
        public CinemachineVirtualCamera MvirtualCamera;
        public CinemachineVirtualCamera LvirtualCamera;
        public CinemachineVirtualCamera TargetVirtualCamera;
        
        //Real pos
        private List<GameObject> playerModels = new List<GameObject>();
        private List<GameObject> enemyModels = new List<GameObject>();
        private List<CinemachineVirtualCamera> CamList = new List<CinemachineVirtualCamera>();

        [SerializeField]
        private TargetGUI _tgui;
        private List<GameObject> TargetGUIList = new List<GameObject>();

        public float spacing = 3.0f;

        private void Awake()
        {
            //myCameraOffset = CameraOffset.LEFT;
            GeneratePlayerModel(playerModels);
            GenerateEnemyModel(enemyModels);
        }
        private void Update()
        {
            //just for debug
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectCharacterCamera(Random.Range(0, _characterModelSetupList.Count));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectEnemy(Random.Range(0, enemyModelSetupList.Count));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ToggleTargetMode();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ReloadAllModelPoint();
            }
        }
        #region GENERATOR
        private void GenerateEnemyModel(List<GameObject> enemyModels)
        {
            if(_enemyModelSetupList == null)
            {
                return;
            }
            float totalWidth_e = (_enemyModelSetupList.Count - 1) * spacing;
            Vector3 startPositionEnemy = transform.position - new Vector3(totalWidth_e * 0.5f, 0f, 0f);
            //Init Enemy Model
            for (int i = 0; i < enemyModelSetupList.Count; i++)
            {
                Vector3 spawnPosition = startPositionEnemy + new Vector3(i * spacing, 0.5f, 10f);
                Vector3 targetPosition = startPositionEnemy + new Vector3(i * spacing, 2.5f, 10.5f);
                GameObject newPoint = Instantiate(enemyModelSetupList[i], spawnPosition, Quaternion.identity, transform);
                GameObject newTarget = _tgui.Init(targetPosition, newPoint.transform);
                newPoint.transform.rotation = Quaternion.Euler(rotation);
                newPoint.name = "Enemy" + i;
                //enemyPoints.Add(spawnPosition);
                enemyModels.Add(newPoint);
                TargetGUIList.Add(newTarget);
            }
        }
        private void GeneratePlayerModel(List<GameObject> playerModels)
        {
            if (_characterModelSetupList == null)
            {
                return;
            }
            float totalWidth_p = (_characterModelSetupList.Count - 1) * spacing;
            Vector3 startPositionPlayer = transform.position - new Vector3(totalWidth_p * 0.5f, 0f, 0f);
            //Init PlayerCharacter Model
            for (int j = 0; j < _characterModelSetupList.Count; j++)
            {
                Vector3 spawnPosition = startPositionPlayer + new Vector3(j * spacing, 0.5f, -3.0f);
                GameObject newObject = Instantiate(_characterModelSetupList[j], spawnPosition, Quaternion.identity, transform);
                newObject.name = "Player" + j;
                if (j == 0)
                {
                    GenerateVirtualCamera(newObject, CameraOffset.LEFT);
                }
                else if (j != _characterModelSetupList.Count - 1)
                {
                    GenerateVirtualCamera(newObject, CameraOffset.MIDDLE);
                }
                else
                {
                    GenerateVirtualCamera(newObject, CameraOffset.RIGHT);
                }
                playerModels.Add(newObject);
            }
        }
        public void ReloadEnemyModel()
        {
            if(enemyModels != null)
            {
                foreach (GameObject obj in enemyModels)
                {
                    Destroy(obj);
                }
                enemyModels.Clear();
            }
            Debug.Log("Enemy reloaded");
            GenerateEnemyModel(enemyModels);
        }
        public void ReloadPlayerModel()
        {
            if(playerModels != null)
            {
                foreach (GameObject obj in playerModels)
                {
                    Destroy(obj);
                }
                playerModels.Clear();
            }
            Debug.Log("Player reloaded");
            GeneratePlayerModel(playerModels);
        }
        public void ReloadAllModelPoint()
        {
            ReloadEnemyModel();
            ReloadPlayerModel();
        }
        public void GenerateVirtualCamera(GameObject follow, CameraOffset eCam)
        {
            if(follow == null)
            {
                Debug.Log("No follow object");
                return;
            }
            switch (eCam)
            {
                case CameraOffset.LEFT:
                    CinemachineVirtualCamera newVCleft = Instantiate(LvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);
                    GameObject newAimPointleft = new GameObject("AimPoint");
                    newAimPointleft.transform.SetParent(follow.transform);
                    newAimPointleft.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, follow.transform.position.z + 10);
                    newVCleft.Follow = follow.transform;
                    newVCleft.LookAt = newAimPointleft.transform;
                    CamList.Add(newVCleft);
                    break;
                case CameraOffset.MIDDLE:
                    CinemachineVirtualCamera newVCmiddle = Instantiate(MvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);
                    GameObject newAimPointmiddle = new GameObject("AimPoint");
                    newAimPointmiddle.transform.SetParent(follow.transform);
                    newAimPointmiddle.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, follow.transform.position.z + 10);
                    newVCmiddle.Follow = follow.transform;
                    newVCmiddle.LookAt = newAimPointmiddle.transform;
                    CamList.Add(newVCmiddle);
                    break;
                case CameraOffset.RIGHT:
                    CinemachineVirtualCamera newVCright = Instantiate(RvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);
                    GameObject newAimPointright = new GameObject("AimPoint");
                    newAimPointright.transform.SetParent(follow.transform);
                    newAimPointright.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, follow.transform.position.z + 10);
                    newVCright.Follow = follow.transform;
                    newVCright.LookAt = newAimPointright.transform;
                    CamList.Add(newVCright);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region SETUP
        public void SelectCharacterCamera(int playerIndex)
        {
            if(playerIndex > CamList.Count - 1)
            {
                Debug.Log("Out of max player index have!");
                return;
            }
            foreach(CinemachineVirtualCamera vcam in CamList)
            {
                vcam.gameObject.SetActive(false);
            }
            CamList[playerIndex].gameObject.SetActive(true);
        }
        public void SelectEnemy(int enemyindex)
        {
            if (!IsTargetMode)
            {
                return;
            }
            foreach (GameObject tgui in TargetGUIList)
            {
                tgui.gameObject.SetActive(false);
            }
            TargetGUIList[enemyindex].gameObject.SetActive(true);
        }

        public void ToggleTargetMode()
        {
            IsTargetMode = !IsTargetMode;
            foreach (GameObject tgui in TargetGUIList)
            {
                tgui.gameObject.SetActive(false);
            }
            if (!IsTargetMode)
            {
                Debug.Log("Toggle Target Mode: Off");
                return;
            }
            else
            {
                Debug.Log("Toggle Target Mode: On");
                SelectEnemy(Random.Range(0,enemyModelSetupList.Count));
            }
        }
        #endregion
        #region GETSETTER
        [SerializeField]
        public List<GameObject> enemyModelSetupList
        {
            get { return _enemyModelSetupList; }
            set { _enemyModelSetupList = value; }
        }
        [SerializeField]
        public List<GameObject> characterModelSetupList
        {
            get { return _characterModelSetupList; }
            set { _characterModelSetupList = value; }
        }

        public List<GameObject> GetPlayerModelPointList()
        {
            return playerModels;
        }
        public List<GameObject> GetEnemyModelPointList()
        {
            return enemyModels;
        }
        #endregion
    }
}
