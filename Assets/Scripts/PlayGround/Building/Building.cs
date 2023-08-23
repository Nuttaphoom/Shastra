using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    //index = floor, index = 0 means that room is on floor 1
    private List<RoomSO> _rooms = new List<RoomSO>() ;  

    
    public void InsertRoom(RoomSO roomSO)
    {
        ConstructRoom(roomSO) ;
        _rooms.Add(roomSO) ;
    }

    private void ConstructRoom(RoomSO newRoom )
    {
        Vector3 placePos = transform.position; 
        for (int i =0; i < _rooms.Count; i++)
            placePos.y += _rooms[i].GetRoomHeight ;

        placePos.y += newRoom.GetRoomHeight;  

        Instantiate(newRoom.RoomPrefab, placePos, newRoom.RoomPrefab.transform.rotation);
    }
}
