 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
[CreateAssetMenu(fileName = "CardEventChannel", menuName = "ScriptableObject/EventChannels/CardEventChannel")]
public class CardEventChannel : DescriptionBaseSO
{
    public UnityAction<Card> OnEvent;

    public void RaiseEvent(Card _baseCard )
    {
        OnEvent?.Invoke(_baseCard );
    }
}
