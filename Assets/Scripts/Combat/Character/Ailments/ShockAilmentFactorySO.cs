 

using Cinemachine;
using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.PlasticSCM.Editor.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "ShockAilment", menuName = "ScriptableObject/Combat/Ailments/ShcokAilment")]
    public class ShockAilmentFactorySO : AilmentFactorySO
    {
        [SerializeField]
        private ShockAilmentDataType _data;

        public override Ailment FactorizeAilment(CombatEntity patient, int ttl)
        {
            var stunAilment = new ShockAilment(patient, ttl);
            stunAilment.Init(_basicInfo, _data);
            return stunAilment;
        }
    }

    public class ShockAilment : Ailment<ShockAilmentDataType>
    {
        public ShockAilment(CombatEntity patient, int ttl) : base(patient, ttl)
        {

        }
        public override void Init(AilmentBasicDataInfo basicInfo, ShockAilmentDataType dataType)
        {
            this._dataType = dataType;
            this._basicDataInfo = basicInfo;
        }

        public override IEnumerator AilmentRecover()
        {
            yield return DirectorManager.Instance.PlayTimelineCoroutine(_basicDataInfo.RecoverTimelineInfo, new List<GameObject>() { _entity.gameObject });
            _entity.GetComponent<CombatEntityAnimationHandler>().DeAttachVFXFromMeshComponent("SHOCKSAILMENTVFX", "CENTERMESH");

        }

        public override IEnumerator SetEntityAction()
        {
            yield return (TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(_entity, _basicDataInfo.Action.FactorizeRuntimeAction(_entity), true));
        }

        public override void OnApplyAilment()
        {
            _entity.GetComponent<CombatEntityAnimationHandler>().AttachVFXToMeshComponent(_dataType.GetShockStayVFX, "CENTERMESH", "SHOCKSAILMENTVFX");
        }
    }




    [Serializable]
    public struct ShockAilmentDataType
    {
        [SerializeField]
        private GameObject _shock_applier_vfx ;

        [SerializeField]
        private GameObject _shock_stay_vfx;

        public GameObject GetShockApplierVFX => _shock_applier_vfx;
        public GameObject GetShockStayVFX => _shock_applier_vfx;

    }



}
