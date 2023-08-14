
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Vanaring_DepaDemo;
using System.Runtime.CompilerServices;
using CustomYieldInstructions;

[CreateAssetMenu(fileName = "HavocApplierEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/HavocApplierEffectFactorySO")]
public class HavocApplierEffectFactorySO : RuntimeEffectFactorySO
{
    [SerializeField]
    ActionAnimationInfo _actionAnimationInfo;

    [Header("This section should be insert with Evoke status effect")]
    [SerializeField]
    private List<StatusRuntimeEffectFactorySO> _effects;

    [Header("Dmg applied when 'add' Havoc to enemy, not when Evoke")]
    [SerializeField]
    private EDamageScaling dmg;
    public override IEnumerator Factorize(List<CombatEntity> targets)
    {
        HavocApplierRuntimeEffect retEffect = new HavocApplierRuntimeEffect(_effects, _actionAnimationInfo, dmg);
        foreach (CombatEntity target in targets)
            retEffect.AssignTarget(target);

        yield return retEffect;
    }
}


public class HavocApplierRuntimeEffect : StatusEffectApplierRuntimeEffect
{ 
    private EDamageScaling dmg = 0 ;
    public HavocApplierRuntimeEffect(List<StatusRuntimeEffectFactorySO> effects, ActionAnimationInfo actionAnimationInfo, EDamageScaling dmg) : base(effects, actionAnimationInfo)
    {
        this.dmg = dmg; 
    }


    //_cgs can be null =, be careful not assuming he got _cgs 
    public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
    {
        List<IEnumerator> applyEffectCoroutine = new List<IEnumerator>();

       

        int attack = VanaringMathConst.GetATKWithScaling(dmg, caster.StatsAccumulator.GetATKAmount());
         

        foreach (CombatEntity target in _targets)
        {
            CombatEntity tar = target;

            if (target is not IStatusEffectable)
                throw new System.Exception("Assigned target is not IStatusEffectable");


            foreach (StatusRuntimeEffectFactorySO effect in _effects)
            {
                StatusRuntimeEffectFactorySO eff = effect;
                applyEffectCoroutine.Add(tar.CombatEntityAnimationHandler.PlayVFXActionAnimation<StatusRuntimeEffectFactorySO>(_actionAnimationInfo.TargetVfxEntity, (param) => ApplyEffectCoroutine(param, tar), eff));


                tar.LogicHurt(null,attack);
            }
        }


        applyEffectCoroutine.Add(caster.CombatEntityAnimationHandler.PlayActionAnimation(_actionAnimationInfo));


        yield return new WaitAll(caster, applyEffectCoroutine.ToArray());


        yield return null;
    }

    protected override IEnumerator ApplyEffectCoroutine(StatusRuntimeEffectFactorySO effect, CombatEntity target)
    {
        List<IEnumerator> coroutines = new List<IEnumerator>();

        coroutines.Add(target.GetStatusEffectHandler().ApplyNewEffect(effect) ) ;
        coroutines.Add(target.VisualHurt(null,_actionAnimationInfo.TargetTrigerID))  ; 


        yield return new WaitAll(target,coroutines.ToArray()) ; 
    }


}

