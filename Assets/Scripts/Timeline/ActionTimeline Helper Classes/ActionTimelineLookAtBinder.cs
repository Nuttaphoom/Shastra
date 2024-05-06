using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    [Serializable]
    public class ActionTimelineLookAtBinder    
    {
        private List<GameObject> _destroyedWithTimeline = new List<GameObject>(); 

        [Serializable]
        private struct LookAtBinderData
        {
            [SerializeField] 
            public CinemachineVirtualCamera _virtualCameraToChangeLookAt;

            public bool UseEnemiesCenterTarget; 
            
        }

        [SerializeField]
        private List<LookAtBinderData> _lookAtBinderDatas ;
        
        public void DestroySpawnedObj()
        {
            for (int i = _destroyedWithTimeline.Count - 1; i >= 0; i--)
            {
                if (_destroyedWithTimeline[i] != null)
                {
                    MonoBehaviour.Destroy(_destroyedWithTimeline[i]);
                }
            }

            _destroyedWithTimeline.Clear();
        }
        #region Set up look at  

        public void BindLookAtTargetsToEnemies(List<Transform> targetTransform)
        {
            int targetSelectedAmount = targetTransform.Count ;
      
            if (targetSelectedAmount == 0)
                return;

            Transform newLookAt = MonoBehaviour.Instantiate(new GameObject().transform);

            Vector3 averagePos = targetTransform[0].transform.position;


            for (int i = 1; i < targetSelectedAmount; i++)
            {
                averagePos += targetTransform[i].transform.position;
            }

            averagePos.x /= targetSelectedAmount;
            averagePos.y /= targetSelectedAmount;
            averagePos.z /= targetSelectedAmount;

            newLookAt.position = averagePos;

            foreach (var binderData in _lookAtBinderDatas)
            {
                binderData._virtualCameraToChangeLookAt.LookAt = newLookAt; 

                _destroyedWithTimeline.Add(newLookAt.gameObject);
            } 

            newLookAt.gameObject.SetActive(true);

        }

        #endregion
    }


}
