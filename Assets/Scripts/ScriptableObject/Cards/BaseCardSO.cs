using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCardSO : ScriptableObject
{
    [Header("Card Visual Representation")]
    [SerializeField] private string _cardName;
    [SerializeField] private float _cardCost;

    //Card Effect would be implemented in here
    //public abstract void ExecuteCardEffect(Tile tile) ;

    public abstract RuntimeCardEffect FactorizeRuntimeCardEffect (); 
    //Normally, as long as given tile is hovered, we can apply this card
    //But there is some card that required more information than that
 

    #region GETTER
    public string GetCardName => _cardName;
    public float GetCardCost  => _cardCost ; 
    #endregion

}

public abstract class RuntimeCardEffect {
    protected List<Component> _targets; 
    public abstract void ExecuteRuntime( ) ;
    public abstract bool VerifyTarget(List<Component> directTarget ) ;

    public abstract System.Type GetTargetType(); 
}

 

 
