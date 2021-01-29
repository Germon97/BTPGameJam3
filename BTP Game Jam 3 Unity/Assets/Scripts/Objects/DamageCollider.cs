using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField]
    private float knockBack = 20;
    [SerializeField]
    private bool destroyAffterTime = true;
    [SerializeField]
    private bool destroyOnContact = true;

    private void Start()
    {
        if(destroyAffterTime)
            Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitBubble hitBubble = collision.GetComponent<HitBubble>();

        if (hitBubble != null)
        {
            hitBubble.Hit(1, GetComponent<Rigidbody2D>().velocity.normalized * knockBack);
        }

        if(destroyOnContact)
            Destroy(gameObject);
    }
}
