using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MusuemTileBaseComposer : TileBaseComposer
{
    [SerializeField]
    private TileBase cornerTile;

    [SerializeField]
    private TileBase pillar;
    [SerializeField]
    private TileBase arc;

    [SerializeField]
    private TileBaseCollection tunnelTiles;
    [SerializeField]
    private TileBaseCollection tunnelFloorTiles;
    [SerializeField]
    private TileBaseCollection roomFloorTiles;

    public override void Init()
    {
        tunnelTiles.Init();
        tunnelFloorTiles.Init();
        roomFloorTiles.Init();
    }

    public override TileBase GetFloorTileBase(int x, int y, bool tunnel)
    {
        if (tunnel)
            return tunnelFloorTiles.GetTileBase();
        else
            return roomFloorTiles.GetTileBase();
    }

    public override TileBase GetWallTileBase(int x, int y, bool tunnel, bool horizontal, bool corner)
    {
        if (corner)
            return cornerTile;


        if (horizontal && x % 4 == 0)
            return pillar;
        if (!horizontal && y % 4 == 0)
            return pillar;

        if (tunnel)
            return tunnelTiles.GetTileBase();

        return arc;
    }
}
