using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infront : MonoBehaviour
{
    [ContextMenu("Debug")]
    private void Start()
    {
        GetComponent<MeshRenderer>().sortingOrder = 4;
    }
}
