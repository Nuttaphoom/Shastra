using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Vanaring 
{
    public abstract class AilmentFactorySO : ScriptableObject
    {
        [SerializeField]
        protected AilmentBasicDataInfo _basicInfo; 
        public abstract Ailment FactorizeAilment(CombatEntity patient,int ttl);
    }
    public abstract class AilmentFactorySO <DataType> : AilmentFactorySO where DataType : class
    {
        [SerializeField]
        private DataType dataType; 
    }

    public abstract class Ailment
    {
        protected CombatEntity _entity; 

        protected int _ttl = 0;

        public Ailment(CombatEntity entity, int ttl)
        {
            _entity = entity;
            _ttl = ttl; 
        }
        public bool AlimentExpired()
        {
            return (_ttl <= 0);
        }

        public void UpdateTTL()
        {
            _ttl -= 1;
        }

        public abstract void OnApplyAilment(); 
        public abstract IEnumerator SetEntityAction();
        public abstract IEnumerator AilmentRecover(); 
        public abstract Comment GetOnTakeControlComment();
        public abstract Comment GetOnRecoverComment();

    }

    /// <summary>
    /// Ailment status effect will be completely different than normal status effect as it controls behavior of the patient 
    /// Right now in control only when owner's control enter is called 
    /// *** If Ailment is expired ,let's say, in Turn 1 (the effect affects still), it will be removed from combat entity at the begining of Turn 2
    /// </summary>
    public abstract class Ailment<DataType> : Ailment where DataType : struct 
    {
        public AilmentBasicDataInfo _basicDataInfo ;
        protected DataType _dataType;

        public Ailment(CombatEntity entity,int ttl) : base(entity,ttl) 
        { 

        }

        public abstract void Init(AilmentBasicDataInfo _basicDataInfo , DataType dataType);
        public override Comment GetOnTakeControlComment()
        {
            return _basicDataInfo.TakeControlComment  ; 
        }
        public override Comment GetOnRecoverComment()
        {
            return _basicDataInfo.TakeControlComment;
        }


    }

    [Serializable]
    public struct AilmentBasicDataInfo
    {
        [SerializeField]
        private Comment _comment_TakeControl;
        public Comment TakeControlComment => _comment_TakeControl ;

        [SerializeField]
        private Comment _comment_Reocver;
        public Comment RecoverComment => _comment_TakeControl;

        [SerializeField]
        private ActorActionFactory _action;
        public ActorActionFactory Action => _action;

        [SerializeField]
        private TimelineInfo _recoverTimelineInfo;
        public TimelineInfo RecoverTimelineInfo => _recoverTimelineInfo;
    }

    

 
    
}
