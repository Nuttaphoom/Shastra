using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities.StringConstant;
using static UnityEngine.EventSystems.EventTrigger;

namespace Vanaring
{
    public class ControlableEntity : CombatEntity , ICameraAttacher
    {
        [SerializeField]
        private ControlableEntityActionsRegistry _controlableEntityActionRegistry;

        public override IEnumerator PrepareForCombat()
        {
            yield return null;
        }

        public override IEnumerator InitializeEntityIntoCombat()
        {
            throw new NotImplementedException() ; 
        } 


        public override IEnumerator GetAction()
        {
            if (_ailmentHandler.DoesAilmentOccur())
            {
                yield return _ailmentHandler.AlimentControlGetAction();
            }
            else
            {
                yield return null;
            }
        }

        public override IEnumerator TakeControl()
        {
            GetComponent<ItemUserHandler>().FactorizeItemInInventory();

            EnableCamera();

            yield return base.TakeControl();

        }

        public override IEnumerator TakeControlLeave()
        {
            yield return base.TakeControlLeave(); 
            ClearCameraData();

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
            _attachedCamera.Follow = gameObject.transform ; 
            CameraSetUPManager.Instance.EnableCamera(_attachedCamera);
        }

        public void ClearCameraData()
        {
            _attachedCamera.Follow = null ;
            //CameraSetUPManager.Instance.DisableCamera(_attachedCamera);
        }
        public override IEnumerator LoadDataFromDatabase()
        {
            string characterName = CombatCharacterSheet.CharacterName;
            
            List<SpellActionSO> spellList = new List<SpellActionSO>();


            var partyMemberRuntimeData = PersistentPlayerPersonalDataManager.Instance.PartyMemberDataLocator.GetRuntimeData(characterName);
            
            spellList = partyMemberRuntimeData.GetRegisteredSpellActionSO;

            //Debug.Log("loaded spell list.count : " + spellList.Count);
            //foreach (var spell in spellList)
            //{
            //    Debug.Log("spell : " + spell.AbilityName);
            //}
            _controlableEntityActionRegistry.RegisterSpell(spellList);

            yield return null;
        }

        public void TemporaryResetLookAtAndFollow()
        {
            throw new NotImplementedException();
        }



        #endregion

        #region GETTER

        public ControlableEntityActionsRegistry GetControlableEntityActionRegistry => _controlableEntityActionRegistry;


        #endregion
    }
}