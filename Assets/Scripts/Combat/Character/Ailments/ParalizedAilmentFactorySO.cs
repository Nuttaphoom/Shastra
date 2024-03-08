 

using Cinemachine;
using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "ParalizedAilment", menuName = "ScriptableObject/Combat/Ailments/ParalizedAilment")]
    public class ParalizedAilmentFactorySO : AilmentFactorySO
    {
        [SerializeField]
        private ParalizedAilmentAilmentDataType _data;

        public override Ailment FactorizeAilment(CombatEntity patient, int ttl)
        {
            var stunAilment = new ParalizedAilment(patient, ttl);
            stunAilment.Init(_basicInfo, _data);
            return stunAilment;
        }
    }

    public class ParalizedAilment : Ailment<ParalizedAilmentAilmentDataType>
    {
        public ParalizedAilment(CombatEntity patient, int ttl) : base(patient, ttl)
        {

        }
        public override void Init(AilmentBasicDataInfo basicInfo, ParalizedAilmentAilmentDataType dataType)
        {
            this._dataType = dataType;
            this._basicDataInfo = basicInfo;
        }

        public override IEnumerator AilmentRecover()
        {
            yield return DirectorManager.Instance.PlayTimelineCoroutine(_basicDataInfo.RecoverTimelineInfo, new List<GameObject>() { _entity.gameObject });
            _entity.GetComponent<CombatEntityAnimationHandler>().DeAttachVFXFromMeshComponent("SHOCKSAILMENTVFX", "VFXPOS") ;

        }

        public override IEnumerator SetEntityAction()
        {
            yield return (TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(_entity, _basicDataInfo.Action.FactorizeRuntimeAction(_entity), true));
        }

        public override void OnApplyAilment()
        {
            _entity.GetComponent<CombatEntityAnimationHandler>().AttachVFXToMeshComponent(_dataType.GetParalizedStayVFX, "VFXPOS", "SHOCKSAILMENTVFX");
        }

        public override bool ShouldOverwrittenOthers()
        {
            return false; 
        }

        public override bool ResistOverwritten()
        {
            return false; 
        }

        public override AilmentLocator.AilmentType GetAilmentType()
        {
            return AilmentLocator.AilmentType.Paralized;
        }

        public override IEnumerator OnAilmentOverwritten()
        {
            _entity.GetComponent<CombatEntityAnimationHandler>().DeAttachVFXFromMeshComponent("SHOCKSAILMENTVFX", "VFXPOS");
            yield return null; 
        }
    }




    [Serializable]
    public struct ParalizedAilmentAilmentDataType
    {
        [SerializeField]
        private GameObject _Paralized_applier_vfx;

        [SerializeField]
        private GameObject _Paralized_stay_vfx;

        public GameObject GetParalizedApplierVFX => _Paralized_applier_vfx;
        public GameObject GetParalizedStayVFX => _Paralized_stay_vfx;

    }



}
