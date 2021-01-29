using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
[CreateAssetMenu(fileName = "TileBaseCollection", menuName = "TileBaseCollection")]
public class TileBaseCollection : ScriptableObject
{
    public MyTileBase[] tileBases;

    private float totalChance = 0;

    public void Init()
    {
        foreach (MyTileBase tb in tileBases)
        {
            totalChance += tb.probability;
        }

        foreach (MyTileBase tb in tileBases)
        {
            tb.probability /= totalChance;
        }
    }


    public TileBase GetTileBase()
    {
        float ran = Random.Range(0, 1);

        float current = 0;

        for (int i = 0; i < tileBases.Length; i++)
        {
            current += tileBases[i].probability;

            if (ran < current)
                return tileBases[i].baseTile;
        }

        return tileBases[tileBases.Length - 1].baseTile;
    }
}
