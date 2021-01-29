using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeEnemyScript : EnemyBehaviorScript
{
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float minPlayerAttackDistance = 10;
    [SerializeField]
    private float timeBetweenSlides = 4;
    [SerializeField]
    private float slidingTime = 2f;

    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite anticipationSprite;
    [SerializeField]
    private SpriteRenderer spriteRenderer;


    private bool isSliding = false;

    private float currentSlideTime = 0;

    private DamageCollider damageCollider;

    private Vector2 slidingDirection = Vector2.zero;


    private void Start()
    {
        damageCollider = GetComponent<DamageCollider>();
        currentBehavior = enemyBehavior.Idle;

        Init();
    }

    private void Update()
    {
        CheckState();

        switch (currentBehavior)
        {
            case enemyBehavior.Attack:
                AttackRoutine();
                break;
        }
    }

    private void CheckState()
    {
        float distanceToPlayer = (transform.position - PlayerMovement.playerPosition).magnitude;

        if (distanceToPlayer > minPlayerAttackDistance)
            currentBehavior = enemyBehavior.Idle;
        else
        {
            if (currentBehavior == enemyBehavior.Idle)
                SpottedPlayer();
            currentBehavior = enemyBehavior.Attack;
        }
    }

    private void AttackRoutine()
    {
        if (isSliding)
        {
            currentSlideTime += Time.deltaTime;


            Vector2 vel = rigi.velocity;
            vel += slidingDirection * speed * Time.deltaTime;
            rigi.velocity = vel;

            if (currentSlideTime > slidingTime)
            {
                damageCollider.enabled = false;

                isSliding = false;
                currentSlideTime = 0;
            }
        }
        else
        {
            currentSlideTime += Time.deltaTime;
            
            if(currentSlideTime > timeBetweenSlides - 0.6f)
            {
                anim.SetWalking(false);
            }

            if(currentSlideTime > timeBetweenSlides)
            {
                damageCollider.enabled = true;

                slidingDirection = (PlayerMovement.playerPosition - transform.position).normalized;

                anim.SetWalking(true);

                isSliding = true;
                currentSlideTime = 0;
            }
        }
    }
}
