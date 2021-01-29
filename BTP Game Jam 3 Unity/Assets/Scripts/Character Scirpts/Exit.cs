using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Exit : MonoBehaviour
{
    public UnityEvent touchedExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && touchedExit != null)
        {
            touchedExit.Invoke();
        }
    }
}
