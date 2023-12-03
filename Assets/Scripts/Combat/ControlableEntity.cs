using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{
    public class ControlableEntity : CombatEntity , ICameraAttacher
    {
        [Header("Right now we manually assign valid action, TODO : Load from Database")]
        [SerializeField]
        private ControlableEntityActionsRegistry _controlableEntityActionRegistry;

        public override IEnumerator InitializeEntityIntoCombat()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetAction()
        {
            if (_ailmentHandler.DoesAilmentOccur())
            {
                yield return _ailmentHandler.AlimentControlGetAction();
            }
            else
            {
                EnableCamera();

                yield return null;
            }
        }

        public override IEnumerator TakeControl()
        {
            yield return base.TakeControl();
        }

        public override IEnumerator TakeControlLeave()
        {
            yield return base.TakeControlLeave();
            DisableCamera(); 
        }

        public override IEnumerator TurnEnter()
        {
            yield return base.TurnEnter();
        }

        public override IEnumerator TurnLeave()
        {
            yield return base.TurnLeave();
        }

        #region INTERFACE 

        private CinemachineVirtualCamera _attachedCamera; 
        public void AttachCamera(CinemachineVirtualCamera camera)
        {
            _attachedCamera = camera; 
        }

        public void EnableCamera()
        {
            CameraSetUPManager.Instance.EnableCamera(_attachedCamera);
        }

        public void DisableCamera()
        {
            CameraSetUPManager.Instance.DisableCamera(_attachedCamera);
        }

 
        #endregion

        #region GETTER

        public ControlableEntityActionsRegistry GetControlableEntityActionRegistry => _controlableEntityActionRegistry;


        #endregion
    }
}