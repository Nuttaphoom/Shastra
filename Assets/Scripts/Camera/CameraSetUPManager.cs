using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

namespace Vanaring 
{
    public interface ICameraAttacher
    {
        void AttachCamera(CinemachineVirtualCamera camera);
        void EnableCamera();
        void DisableCamera(); 
    }
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
       

        //TempList
        private List<GameObject> playerModels = new List<GameObject>();
        private List<GameObject> enemyModels = new List<GameObject>();
        //private List<GameObject> TargetGUIList = new List<GameObject>();
        private List<CinemachineVirtualCamera> CamList = new List<CinemachineVirtualCamera>();

        [SerializeField]
        private Transform _ally_cam_aimPoint;

        [SerializeField]
        private TargetGUI _tgui;

        private GameObject _savedVMCamera = null;
        private Transform _oldAimPoint;


        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject); 
            
            Instance = this;
        }

        //public void SetBlendMode(CameraBlendMode mode, float blendDuration)
        //{
        //    CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        //    if (cinemachineBrain != null)
        //    {
        //        if(mode == CameraBlendMode.EASE_INOUT)
        //        {
        //            cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        //            cinemachineBrain.m_DefaultBlend.m_Time = blendDuration;
        //        }
        //        else
        //        {
        //            cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning("CinemachineBrain component not found on this GameObject.");
        //    }
        //}

        public void SetLookAtTarget(Transform lookat)
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.LookAt = lookat;
        }

        public void CaptureVMCamera()
        {
            _savedVMCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject ;
            _oldAimPoint = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.LookAt;
        }

        public void RestoreVMCameraState()
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);

            _savedVMCamera.gameObject.SetActive(true);
            _savedVMCamera.GetComponent<CinemachineVirtualCamera>().LookAt = _oldAimPoint; 
        }



        #region GENERATOR
        public void GenerateEntityAttacher(List<GameObject> _characterModelSetupList, List<GameObject> _enemyModelSetupList)
        {
            GeneratePlayerAttacher(_characterModelSetupList);
        }
      
        private void GeneratePlayerAttacher(List<GameObject> _characterModelSetupList)
        {
            if (_characterModelSetupList == null)
            {
                return;
            }
             
            for (int j = 0; j < _characterModelSetupList.Count; j++)
            {
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
            }
        }
         
        public void GenerateVirtualCamera(GameObject follow, CameraOffset eCam)
        {
            if(follow == null)
                throw new System.Exception("No follow object");

            CinemachineVirtualCamera newVMCamera = null;

            switch (eCam)
            {
                case CameraOffset.LEFT:
                    newVMCamera  = Instantiate(LvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);

                    break;
                case CameraOffset.MIDDLE:
                    newVMCamera = Instantiate(MvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);

                    break;
                case CameraOffset.RIGHT:
                    newVMCamera = Instantiate(RvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);

                    break;
                 
            }           
            
            newVMCamera.Follow = follow.transform;
            newVMCamera.LookAt = _ally_cam_aimPoint.transform;
            CamList.Add(newVMCamera);

            if (follow.TryGetComponent(out ICameraAttacher iCamAttacher))
                iCamAttacher.AttachCamera(newVMCamera);

            

            //switch (eCam)
            //{
            //    case CameraOffset.LEFT:
            //        CinemachineVirtualCamera newVCleft = Instantiate(LvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);
            //        GameObject newAimPointleft = new GameObject("AimPoint");
            //        newAimPointleft.transform.SetParent(follow.transform);
            //        newAimPointleft.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, follow.transform.position.z  );
            //        newVCleft.Follow = follow.transform;
            //        newVCleft.LookAt = newAimPointleft.transform;
            //        CamList.Add(newVCleft);
            //        break;
            //    case CameraOffset.MIDDLE:
            //        CinemachineVirtualCamera newVCmiddle = Instantiate(MvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);
            //        GameObject newAimPointmiddle = new GameObject("AimPoint");
            //        newAimPointmiddle.transform.SetParent(follow.transform);
            //        newAimPointmiddle.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, follow.transform.position.z  );
            //        newVCmiddle.Follow = follow.transform;
            //        newVCmiddle.LookAt = newAimPointmiddle.transform;
            //        CamList.Add(newVCmiddle);
            //        break;
            //    case CameraOffset.RIGHT:
            //        CinemachineVirtualCamera newVCright = Instantiate(RvirtualCamera, gameObject.transform.position, Quaternion.identity, follow.transform);
            //        GameObject newAimPointright = new GameObject("AimPoint");
            //        newAimPointright.transform.SetParent(follow.transform);
            //        newAimPointright.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, follow.transform.position.z);
            //        newVCright.Follow = follow.transform;
            //        newVCright.LookAt = newAimPointright.transform;
            //        CamList.Add(newVCright);
            //        break;
            //    default:
            //        break;
            //}
        }
        #endregion
        #region MODE
        
        public void EnableCamera(CinemachineVirtualCamera newCamera)
        {
            if (Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera != null)
                Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false) ;  
            
            newCamera.gameObject.SetActive(true) ; 
        }

        public void DisableCamera(CinemachineVirtualCamera newCamera)
        {
            newCamera.gameObject.SetActive(false) ; 
        }



        #endregion

    }
}
