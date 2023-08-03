
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Vanaring_DepaDemo;
using System.Runtime.CompilerServices;
using CustomYieldInstructions;

[CreateAssetMenu(fileName = "StatusEffectApplierFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffectApplierFactorySO")]
public class StatusEffectApplierFactorySO : RuntimeEffectFactorySO
{
    [SerializeField]
    ActionAnimationInfo _actionAnimationInfo; 

    [SerializeField]
    private List<StatusRuntimeEffectFactorySO> _effects;
    public override IEnumerator Factorize(List<CombatEntity> targets)
    {
        StatusEffectApplierRuntimeEffect retEffect = new StatusEffectApplierRuntimeEffect(_effects, _actionAnimationInfo) ; 
        foreach (CombatEntity target in targets)
            retEffect.AssignTarget(target);

        yield return retEffect   ;
    }
}


public class StatusEffectApplierRuntimeEffect : RuntimeEffect
{
    [SerializeField]
    private List<StatusRuntimeEffectFactorySO> _effects;

    private ActionAnimationInfo _actionAnimationInfo; 
    public StatusEffectApplierRuntimeEffect(List<StatusRuntimeEffectFactorySO> effects, ActionAnimationInfo actionAnimationInfo)
    {
        _effects = effects;
        _actionAnimationInfo = actionAnimationInfo;  
    }


    //_cgs can be null =, be careful not assuming he got _cgs 
    public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
    {
        List<IEnumerator> applyEffectCoroutine = new List<IEnumerator>(); 
        foreach (CombatEntity target in _targets)
        {
            CombatEntity tar = target; 

            if (target is not IStatusEffectable)
                throw new System.Exception("Assigned target is not IStatusEffectable");

            foreach (StatusRuntimeEffectFactorySO effect in _effects)
            {
                StatusRuntimeEffectFactorySO eff = effect; 
                applyEffectCoroutine.Add((tar).GetStatusEffectHandler().ApplyNewEffect(eff, _actionAnimationInfo) ) ;
            }
        }

        applyEffectCoroutine.Add(caster.CombatEntityAnimationHandler.PlayActionAnimation(_actionAnimationInfo));


        yield return new WaitAll(caster, applyEffectCoroutine.ToArray()); 

        
        yield return null;
    }

    
}

