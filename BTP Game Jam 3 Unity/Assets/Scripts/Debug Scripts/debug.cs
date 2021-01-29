using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    [ContextMenu("debug")]
    private void de()
    {
        Room r1 = new Room(new RectAnglePrimitive(new Vector2Int(0,0), new Vector2Int(0,0)));
        Room r2 = new Room(new RectAnglePrimitive(new Vector2Int(0, 0), new Vector2Int(0, 0)));
        Debug.LogWarning(r1.IsConnectedTo(r2));
        r1.ConnectNewRoom(r2);
        Debug.LogWarning(r1.IsConnectedTo(r2));
    }
}
