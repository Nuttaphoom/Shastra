using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaygroundBoard : MonoBehaviour
{
    public enum PlayerSide
    {
        PlayerOne,
        PlayerTwo
    }

    [Header("Static Ground Data : Should be on edtior mode not runtime")]
    [SerializeField]
    private PlaygroundBoard.PlayerSide _playerSide;

    [Header("For sake of testing, we now manually assign tiles")]
    [SerializeField]
    private List<Tile> _tiles ;

    #region EventListening
 
    #endregion
    public void BoardInit()
    {
        //TODO, Maybe use Tile prefab to Init  
    }


}
