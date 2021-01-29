using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MyTileBase
{
    public TileBase baseTile;
    [Range(0,1)]
    public float probability;
}
