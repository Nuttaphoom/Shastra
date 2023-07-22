
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Vanaring_DepaDemo;
using System.Runtime.CompilerServices;

[CreateAssetMenu(fileName = "StatusEffectApplierFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffectApplierFactorySO")]
public class StatusEffectApplierFactorySO : RuntimeEffectFactorySO
{
    [SerializeField]
    private List<StatusRuntimeEffectFactorySO> _effects;
    public override IEnumerator Factorize(List<CombatEntity> targets)
    {
        StatusEffectApplierRuntimeEffect retEffect = new StatusEffectApplierRuntimeEffect(_effects) ; 
        foreach (CombatEntity target in targets)
            retEffect.AssignTarget(target);

        yield return retEffect   ;
    }
}


public class StatusEffectApplierRuntimeEffect : RuntimeEffect
{
    [SerializeField]
    private List<StatusRuntimeEffectFactorySO> _effects;

    public StatusEffectApplierRuntimeEffect(List<StatusRuntimeEffectFactorySO> effects)
    {
        _effects = effects;
    }


    //_cgs can be null =, be careful not assuming he got _cgs 
    public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
    {
        foreach (CombatEntity target in _targets)
        {
            if (target is not IStatusEffectable)
                throw new System.Exception("Assigned target is not IStatusEffectable");

            foreach (StatusRuntimeEffectFactorySO effect in _effects)
            {
                Debug.Log("yield return to  " + target) ;
                yield return (target).TakeControl(); 
                
                yield return (target).GetStatusEffectHandler().ApplyNewEffect(effect);
                Debug.Log("finish yield return"); 
            }
        }
        yield return null;
    }

    
}

