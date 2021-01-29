using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Shadow Map", menuName = "Shadow Map")]
public class ShadowMap : ScriptableObject
{
    public GameObject single;
    public GameObject horizontal;
    public GameObject vertical;
    public GameObject downSingle;
    public GameObject upSingle;
    public GameObject leftSingle;
    public GameObject rightSingle;
    public GameObject downRightCorner;
    public GameObject downLeftCorner;
    public GameObject upRightCorner;
    public GameObject upLeftCorner;
    public GameObject cross;


    /// <summary>
    /// Returns the right shadow map for a coded 3x3 tile.
    /// X 8 X
    /// 2 O 4
    /// X 1 X
    /// Coded binary.
    /// </summary>
    /// <returns></returns>
    public GameObject GetShadow(int code)
    {
        switch (code)
        {
            case 1: return downSingle;
            case 2: return leftSingle;
            case 3: return downLeftCorner;
            case 4: return rightSingle;
            case 5: return downRightCorner;
            case 6:
            case 7:
            case 14: return horizontal;
            case 8: return upSingle;
            case 9:
            case 11:
            case 13: return vertical;
            case 10: return upLeftCorner;
            case 12: return upRightCorner;
            case 15:
                return cross;
            case 0:
            default:
                return single;
        }
    }
}
