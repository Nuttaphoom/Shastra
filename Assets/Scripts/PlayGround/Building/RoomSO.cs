using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomSO" , menuName = "ScriptableObject/Room/RoomSO")]
public class RoomSO : ScriptableObject
{
    [Header("Prefab for a room")]
    [SerializeField]
    private GameObject _roomPrefab;

    [Header("Parameters for adjust room placment")]
    [SerializeField]
    //Room height is actual room height / 2 
    private float _roomHeight ;

    #region GETTER 
    public GameObject RoomPrefab => _roomPrefab;

    public float GetRoomHeight => _roomHeight;  
    #endregion


}
