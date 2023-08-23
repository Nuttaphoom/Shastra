using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events; 
//Card class responsible for basic card operation like draging around, or release
//Actual effect of each card will be implemented in CardSO
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Contain visual,and effect of the card 
    private BaseCardSO _cardSO;
    [Header("Card Visual")]
    [SerializeField]
    private TextMeshProUGUI _cardNameTMP; 

    [Header("Broadcast to")]
    [SerializeField]
    private CardEventChannel OnHoldingCardEvent ;
    [SerializeField]
    private CardEventChannel OnReleaseCardEvent ;
    [SerializeField]
    private CardEventChannel OnPointerEnterCardEvent;
    [SerializeField]
    private CardEventChannel OnPointerExitCardEvent ;
    
    //Listen by   CardVisual 
    //Broadcast by CardManager
    public UnityAction<bool> TogglePointerInteractionEvent; 

    private bool _enablePointerFunc = true ;
    
    public UnityAction<Card,Vector2> OnDragCardEvent;

 

    public void CardInit(BaseCardSO cardSO)
    {
        _cardSO = cardSO; ;
        _cardNameTMP.text = _cardSO.GetCardName ;  
    }

    //Card Logic 
    //Maybe we use effect factory to cast a card ability ? not sure 
    public RuntimeCardEffect FactorizeRuntimeCardEffect(   )
    {
        RuntimeCardEffect  rce = _cardSO.FactorizeRuntimeCardEffect( ) ;
        return rce;
    }

   

    #region EventListener 
 
    #endregion
    #region EventBroadcast

    public void OnTogglePointerInteraction(bool b)
    {
        TogglePointerInteractionEvent?.Invoke(b);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnHoldingCardEvent.RaiseEvent(this); 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleaseCardEvent.RaiseEvent(this) ;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
 
        OnDragCardEvent?.Invoke(this,eventData.delta) ;  
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterCardEvent.RaiseEvent(this);  
    }

    public void OnPointerExit(PointerEventData eventData)
    {
 

        OnPointerExitCardEvent.RaiseEvent(this);  
    }


    #endregion

    #region GETTER 
    public BaseCardSO CardData => _cardSO;
    #endregion   

}
