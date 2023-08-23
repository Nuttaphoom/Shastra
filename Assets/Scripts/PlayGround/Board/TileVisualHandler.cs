using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tile))] 
public class TileVisualHandler : MonoBehaviour
{
    private Tile _tile ;

    [Header("Variable for manipulating tile visual")]
    [SerializeField]
    private SpriteRenderer _rendererSprite ;  

    private void Awake()
    {
        _tile = GetComponent<Tile>();  
    }
    private void OnEnable()
    {
        _tile.MouseEnterTileEvent += HightLightTile;
        _tile.MouseExitTileEvent += UnHightlightTile;
    }

    private void OnDisable()
    {
        _tile.MouseEnterTileEvent -= HightLightTile;
        _tile.MouseExitTileEvent -= UnHightlightTile;
    }

    
    public void HightLightTile()
    {
        //For now just change color of sprite  
        _rendererSprite.color = Color.yellow; 
    }

    public void UnHightlightTile()
    {
        _rendererSprite.color = Color.white; 
    }


}
