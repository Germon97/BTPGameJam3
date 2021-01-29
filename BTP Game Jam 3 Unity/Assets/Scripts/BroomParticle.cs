using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomParticle : DamageCollider
{
    [SerializeField]
    private float topSpeed;
    [SerializeField]
    private AnimationCurve speed;
    [SerializeField]
    private float lifeTime;

    private Vector2 direction;
    private Rigidbody2D rigi;
    private float currentLifeTime = 0;

    private void Start()
    {
        GetComponent<Animator>().SetFloat("LifeTime", 1 / lifeTime);
        rigi = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        Vector2 vel = rigi.velocity;
        vel = direction * speed.Evaluate(currentLifeTime / lifeTime) * topSpeed * Time.deltaTime;
        rigi.velocity = vel;
    }
}
