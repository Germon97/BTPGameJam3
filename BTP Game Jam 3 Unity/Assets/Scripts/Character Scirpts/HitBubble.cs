using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBubble : MonoBehaviour
{
    private HealthScript healthScript;
    private Rigidbody2D rigi;

    private void Start()
    {
        healthScript = GetComponentInParent<HealthScript>();
        rigi = GetComponentInParent<Rigidbody2D>();
    }

    public void Hit(int damage, Vector2 knockBack)
    {
        healthScript.ChangeHealth(-damage);
        rigi.AddForce(knockBack, ForceMode2D.Impulse);
    }
}
