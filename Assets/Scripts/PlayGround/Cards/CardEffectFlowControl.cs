using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardEffectFlowControl   
{
    public IEnumerator RunCardEffectFlowCoroutine(CardManager cardManager, Card card, List<Component> directTarget = null )
    {
        //1 Get Target first 
        RuntimeCardEffect runtimeEffect =  card.FactorizeRuntimeCardEffect() ;
        if (runtimeEffect.VerifyTarget(directTarget))
            goto ApplyCardEffectDest;
        else
        {
            //TODO Make runtime effect call SelectTargetCorutine
            throw new System.Exception("Selecting target after applying card is not avraible for now"); 
        }

    //2 Apply card effect
    ApplyCardEffectDest:
        runtimeEffect.ExecuteRuntime();
        //TODO Try to get rid of this line, no need to reference to CardManager in here if we can tell
        //that the card is applied.
        //maybe use EventChannel OnApplyCard ? make this one monobehavior
        cardManager.DiscardCard(card); 

        yield return null; 
    }

    //be called in RunTimeEffect 
    public IEnumerator SelectTargetCourutine<T>()
    {
        
        yield return null; 
    }
}
