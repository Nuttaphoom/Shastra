using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Tile : MonoBehaviour  
{
    private bool _isCursorHover = false;

    //Building related varaibles 
    public Building _building  ;

    //Boardcast by this, listen by associated building, and tile visual 
    public UnityAction MouseEnterTileEvent;  
    public UnityAction MouseExitTileEvent ;

    public void Build(RoomSO roomSO)
    {
        _building.InsertRoom(roomSO); 
    }

 

    #region EventBroadcaster 
    private void CursorEnterCollision()
    {
        MouseEnterTileEvent?.Invoke();
    }
    private void CursorExitCollision()
    {
        MouseExitTileEvent?.Invoke(); 
    }

    #endregion  

    #region BuiltInEvent 

    private void OnMouseEnter()
    {
        _isCursorHover = true; 

        CursorEnterCollision(); 
    }

    private void OnMouseExit()
    {
        _isCursorHover = false; 

        CursorExitCollision(); 
    }
    #endregion 


}
