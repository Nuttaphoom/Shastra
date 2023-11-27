﻿using Cinemachine;
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
    [CreateAssetMenu(fileName = "StunAilment" , menuName = "ScriptableObject/Combat/Ailments/StunAilment")]
    public class StunAilmentFactorySO : AilmentFactorySO
    {
        [SerializeField]
        private StunAilmentDataType _data ; 
        
        public override Ailment FactorizeAilment(CombatEntity patient,int ttl)
        {
            var stunAilment = new StunAilment(patient, ttl);
            stunAilment.Init(_basicInfo,_data);
            return stunAilment;
        }
    }

    public class StunAilment : Ailment<StunAilmentDataType>
    {
        public StunAilment(CombatEntity patient,int ttl) : base(patient,   ttl) 
        {

        }
        public override void Init(AilmentBasicDataInfo basicInfo, StunAilmentDataType dataType)
        {
            this._dataType = dataType;
            this._basicDataInfo = basicInfo;
            this._basicDataInfo.header = "somethingggg";
        }
        
        public override IEnumerator AilmentRecover()
        {
           yield return DirectorManager.Instance.PlayTimelineCoroutine(_basicDataInfo.RecoverTimelineInfo, new List<GameObject>() {_entity.gameObject})  ; 
           yield return _entity.GetComponent<EnergyOverflowHandler>().ResetOverflow() ;
        }



        public override IEnumerator SetEntityAction()
        {
            yield return  (TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(_entity, _basicDataInfo.Action.FactorizeRuntimeAction(_entity),true));
        }
    }

    [Serializable]
    public struct StunAilmentDataType  
    {
   

     }






}
