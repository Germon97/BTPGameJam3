using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class triggering a method when the level is reset.
/// </summary>
public class WipeTrigger : MonoBehaviour
{
    public UnityEvent WipeCallBack;

    private void OnEnable()
    {
        if (GameManager.instance != null)
            GameManager.instance.WipeLevel.AddListener(TriggerEvent);
    }

    private void OnDisable()
    {
        if (GameManager.instance != null)
            GameManager.instance.WipeLevel.AddListener(TriggerEvent);
    }

    private void TriggerEvent()
    {
        if (WipeCallBack != null)
            WipeCallBack.Invoke();
    }
}
