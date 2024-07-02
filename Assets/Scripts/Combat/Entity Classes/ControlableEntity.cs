using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Vanaring
{
    public class ControlableEntity : CombatEntity , ICameraAttacher
    {
        [SerializeField]
        private ControlableEntityActionsRegistry _controlableEntityActionRegistry;

        [SerializeField]
        private RuntimePartyMember _runtimePartyMemberData;

       
        public void LinkPartyMemberToThisEntity(RuntimePartyMember partyMember)
        {
            _runtimePartyMemberData = partyMember; 
        }

        public override IEnumerator InitializeEntityIntoCombat()
        {
            yield return base.InitializeEntityIntoCombat();

            //Set up runtime value according to Party member data
            if (_runtimePartyMemberData != null)
            {
                _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_runtimePartyMemberData);
                _spellCaster.SetNewMPAttribute(_runtimePartyMemberData.GetCurrentPartyMemberMP, _runtimePartyMemberData.GetRuntimeCombatMemberData.LevelAttributeHandler.GetSecondaryAttribute_MaxMP);

            }
            else
            {
                _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_characterSheet);
                _spellCaster.SetNewMPAttribute(_characterSheet.Get_Base_SecondaryAttribute_MaxMP, _characterSheet.Get_Base_SecondaryAttribute_MaxMP);

            }
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
    
            EnableCamera() ;

            EntityPositionManager.Instance.SetNewEnemyCurrentSize(CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Hostile).Count);
            EntityPositionManager.Instance.OccupieLocation(ECompetatorSide.Ally,0,this) ;


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

        public override IEnumerator OnPerformAction()
        {
            yield return base.OnPerformAction();

            ClearCameraData();
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
             _attachedCamera.Follow = GetComponent<CombatEntityAnimationHandler>().GetCenterMesh().transform ; 
            CameraSetUPManager.Instance.EnableCamera(_attachedCamera);
        }

        public void ClearCameraData()
        {
            _attachedCamera.Follow = null ;
            _attachedCamera.gameObject.SetActive(false);    
            //CameraSetUPManager.Instance.DisableCamera(_attachedCamera);
        }
        public override IEnumerator LoadDataFromDatabase()
        {
            string characterName = CombatCharacterSheet.CharacterName;

            RuntimeCombatMemberData partyMemberRuntimeData = PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeData(characterName);

            LoadUnLockedSpell(partyMemberRuntimeData) ;
            //LoadBreakTriggerHandler(partyMemberRuntimeData);

            yield return null;
        }

        private void LoadUnLockedSpell(RuntimeCombatMemberData partyMemberRuntimeData)
        {
            List<SpellActionSO> spellList = new List<SpellActionSO>();

            spellList = partyMemberRuntimeData.GetRegisteredSpellActionSO;
            _controlableEntityActionRegistry.RegisterSpell(spellList);

        }
         
        private void LoadBreakTriggerHandler(RuntimeCombatMemberData partyMemberRuntimeData)
        {
            List<TriggerActionSO> triggerList = partyMemberRuntimeData.GetTriggerActionSOs ; 
            _breakTriggerHandler.LoadTriggerActionFromDatabase(triggerList); 
        }



        #endregion

        #region GETTER

        public ControlableEntityActionsRegistry GetControlableEntityActionRegistry => _controlableEntityActionRegistry;


        #endregion
    }
}