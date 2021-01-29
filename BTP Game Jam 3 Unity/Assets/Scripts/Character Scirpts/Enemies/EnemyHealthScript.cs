using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviorScript))]
public class EnemyHealthScript : HealthScript
{
    private void Start()
    {
        Init();
        GotKilled.AddListener(Kill);
    }

    private void Kill()
    {
        GetComponent<EnemyBehaviorScript>().Dead();
    }
}
