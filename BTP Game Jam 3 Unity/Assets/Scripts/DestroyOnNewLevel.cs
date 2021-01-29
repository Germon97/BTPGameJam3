using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnNewLevel : MonoBehaviour
{
    private void Start()
    {

        if (GameManager.instance != null)
        {
            GameManager.instance.WipeLevel.AddListener(Kill);
        }
    }

    private void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.WipeLevel.AddListener(Kill);
        }
    }

    private void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.WipeLevel.RemoveListener(Kill);
        }
    }

    private void Kill()
    {
        Destroy(this.gameObject);
    }
}
