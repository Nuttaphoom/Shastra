using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for holding data only.

[CreateAssetMenu(fileName = "DeckSO", menuName = "ScriptableObject/DeckSO")]
public class Deck : ScriptableObject    
{
    enum Owner { 
        Player1, 
        Player2,
    };

    [SerializeField]
    private Owner _lable;

    [Header("For testing, right now we can set cardSO manually")]
    [SerializeField]
    private BaseCardSO[] _cardSOs ;

    //Variable for deck managment 
    [SerializeField]
    private int _currentIndex = 0;
    public void LoadCardDataIntoDeck(BaseCardSO[] _cardData )
    {
        //TODO : Need to be called in DeckLoader

    }

    public BaseCardSO DrawCard()
    {
        BaseCardSO c =  (_cardSOs[_currentIndex]) ;
        _currentIndex += 1;
        _currentIndex = _currentIndex % _cardSOs.Length;
       
        return c; 
    }

  
}
