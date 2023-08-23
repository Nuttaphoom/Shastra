using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

namespace Vanaring_DepaDemo
{
    public class CameraSetUPManager : MonoBehaviour
    {
        public enum CameraBlendMode { EASE_INOUT, CUT}
        public static CameraSetUPManager Instance; 
        public enum CameraOffset { LEFT, MIDDLE, RIGHT };
        private bool IsTargetMode = false;

        private CinemachineVirtualCamera _actionCamera;

        [SerializeField] private CinemachineBrain cinemachineBrain;

        [Header("List of character/enemy order from left to right")]
        public List<GameObject> _enemyModelSetupList = new List<GameObject>();
        public List<GameObject> _characterModelSetupList = new List<GameObject>();
        
        //Camera on each behavior
        [Header("Camera Type")]
        [SerializeField] private CinemachineVirtualCamera RvirtualCamera;
        [SerializeField] private CinemachineVirtualCamera MvirtualCamera;
        [SerializeField] private CinemachineVirtualCamera LvirtualCamera;
        [SerializeField] private CinemachineVirtualCamera targetVirtualCamera;

        //TempList
        private List<GameObject> playerModels = new List<GameObject>();
        private List<GameObject> enemyModels = new List<GameObject>();
        private List<GameObject> TargetGUIList = new List<GameObject>();
        private List<CinemachineVirtualCamera> CamList = new List<CinemachineVirtualCamera>();

        [SerializeField]
        private TargetGUI _tgui;

        //public GameObject tempEntity;
        //public GameObject tempEntity1;

        private GameObject _savedVMCamera = null;

        [Header("Shaking Camera Properties")]
        private CinemachineVirtualCamera shakedVirtualCamera;
        private float shakeDuration = 3.0f;
        private float shakeAmplitude = 1.2f;
        private float shakeFrequency = 2.0f;
        private float shakeTimer = 0.0f;
        private bool isShaking = false;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject); 
            
            Instance = this;
        }
        private void Start()
        {
            isShaking = false;
        }
        private void Update()
        {
            //Debug.Log(Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>());
            //if (isShaking)
            //{
            //    shakeTimer -= Time.deltaTime;
            //    if (shakeTimer <= 0.0f)
            //    {
            //        var noise = shakedVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //        noise.m_AmplitudeGain = 0.0f;
            //        noise.m_FrequencyGain = 0.0f;
            //        isShaking = false;
            //    }
            //}
        }

        

        private IEnumerator shakeVirtualCamera(CinemachineVirtualCamera shakedVirtualCamera)
        {
            while (shakeTimer > 0.0f && isShaking)
            {
                shakeTimer -= Time.deltaTime;
                yield return null;
            }

            var noise = shakedVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0.0f;
            noise.m_FrequencyGain = 0.0f;
            isShaking = false;
        }

        public void SetBlendMode(CameraBlendMode mode, float blendDuration)
        {
            CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

            if (cinemachineBrain != null)
            {
                if(mode == CameraBlendMode.EASE_INOUT)
                {
                    cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
                    cinemachineBrain.m_DefaultBlend.m_Time = blendDuration;
                }
                else
                {
                    cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
                }
            }
            else
            {
                Debug.LogWarning("CinemachineBrain component not found on this GameObject.");
            }
        }
        
        public void SetupAttackActionVirtualCamera(GameObject entity)
        {
            DeactivateAllVirtualCameras();
            entity.GetComponent<CombatEntityAnimationHandler>().ActionCamera.gameObject.SetActive(true);
        }

        public void ActiveTargetModeVirtualCamera()
        {
            
                DeactivateAllVirtualCameras();
                targetVirtualCamera.gameObject.SetActive(true);
             
        }

        public void SetupTargatModeLookAt(GameObject entity)
        {
            if (targetVirtualCamera.LookAt != entity.transform) 
                targetVirtualCamera.LookAt = entity.transform;
        }

        private void DeactivateAllVirtualCameras()
        {
            CinemachineVirtualCamera[] virtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();

            foreach (CinemachineVirtualCamera camera in virtualCameras)
            {
                camera.gameObject.SetActive(false);
            }
        }

        public void CaptureVMCamera()
        {
            _savedVMCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject ;   
        }
  
        public void RestoreVMCameraState()
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false); 
            _savedVMCamera.gameObject.SetActive(true);
        }

        #region GENERATOR
        public void GenerateEntityAttacher(List<GameObject> _characterModelSetupList, List<GameObject> _enemyModelSetupList)
        {
            GeneratePlayerAttacher(_characterModelSetupList);
            GenerateEnemyAttacher(_enemyModelSetupList);
        }
        private void GenerateEnemyAttacher(List<GameObject> _enemyModelSetupList)
        {
            if(_enemyModelSetupList == null)
            {
                return;
            }
            //float totalWidth_e = (_enemyModelSetupList.Count - 1) * spacing;
            //Vector3 startPositionEnemy = transform.position - new Vector3(totalWidth_e * 0.5f, 0f, 0f);
            //Init Enemy Model
            for (int i = 0; i < _enemyModelSetupList.Count; i++)
            {
                //Vector3 spawnPosition = startPositionEnemy + new Vector3(i * spacing, 0.5f, 10f);
                //Vector3 targetPosition = enemyModelSetupList[i].transform.position + new Vector3(i * spacing, 2.5f, 10.5);
                Vector3 targetPosition = _enemyModelSetupList[i].transform.position + new Vector3(0, 0, -1.2f);
                //GameObject newPoint = Instantiate(enemyModelSetupList[i], spawnPosition, Quaternion.identity, transform);
                GameObject newTarget = _tgui.Init(targetPosition, _enemyModelSetupList[i].transform);
                //newPoint.transform.rotation = Quaternion.Euler(rotation);
                //newPoint.name = "Enemy" + i;
                //enemyPoints.Add(spawnPosition);
                //enemyModels.Add(newPoint);
                TargetGUIList.Add(newTarget);
            }
        }
        private void GeneratePlayerAttacher(List<GameObject> _characterModelSetupList)
        {
            if (_characterModelSetupList == null)
            {
                return;
            }
            //float totalWidth_p = (_characterModelSetupList.Count - 1) * spacing;
            //Vector3 startPositionPlayer = transform.position - new Vector3(totalWidth_p * 0.5f, 0f, 0f);
            //Init PlayerCharacter Model
            for (int j = 0; j < _characterModelSetupList.Count; j++)
            {
                //Vector3 spawnPosition = startPositionPlayer + new Vector3(j * spacing, 0.5f, -3.0f);
                //GameObject newObject = Instantiate(_characterModelSetupList[j], spawnPosition, Quaternion.identity, transform);
                //newObject.name = "Player" + j;
                if (j == 0)
                {
                    GenerateVirtualCamera(_characterModelSetupList[j], CameraOffset.LEFT);
                }
                else if (j != _characterModelSetupList.Count - 1)
                {
                    GenerateVirtualCamera(_characterModelSetupList[j], CameraOffset.MIDDLE);
                }
                else
                {
                    GenerateVirtualCamera(_characterModelSetupList[j], CameraOffset.RIGHT);
                }
                //playerModels.Add(newObject);
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
            GenerateEnemyAttacher(_enemyModelSetupList);
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
            GeneratePlayerAttacher(_characterModelSetupList);
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
        #region MODE
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
 

  
        #endregion
        #region GETSETTER
        //[SerializeField]
        public List<GameObject> EnemyModelSetupList
        {
            get { return _enemyModelSetupList; }
            set { _enemyModelSetupList = value; }
        }
        [SerializeField]
        public List<GameObject> CharacterModelSetupList
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
