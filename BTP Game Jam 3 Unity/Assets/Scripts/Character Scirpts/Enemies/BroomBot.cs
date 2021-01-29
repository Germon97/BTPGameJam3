using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BroomBot : EnemyBehaviorScript
{
    [SerializeField]
    private float playerDetectRange = 7;
    [SerializeField]
    private float normalSpeed = 9;
    [SerializeField]
    private float attackSpeed = 9;

    [SerializeField]
    private Animator light;
    [SerializeField]
    private Animator broomAnimator;
    [SerializeField]
    private Transform broomHolder;

    [SerializeField]
    private float broomingTime;
    private float currentTime;
    [SerializeField]
    private float waitTime;

    [SerializeField]
    private GameObject brushParticlesPrefab;
    [SerializeField]
    private Transform particleSpawn;

    private bool attackBrooming;

    private float timeSinceLastBrush = 0;

    private void Start()
    {
        Init();
        currentBehavior = enemyBehavior.Idle;
    }

    private void Update()
    {
        if (currentBehavior == enemyBehavior.Dead)
            return;

        PointBroom();
        CheckState();

        switch (currentBehavior)
        {
            case enemyBehavior.Wait:
                break;
            case enemyBehavior.Attack:
                Attack();
                break;
        }
    }

    private void CheckState()
    {
        float distance = (transform.position - PlayerMovement.playerPosition).magnitude;

        if (distance < playerDetectRange)
        {
            if (currentBehavior == enemyBehavior.Idle)
                SpottedPlayer();

            currentBehavior = enemyBehavior.Attack;
            light.gameObject.SetActive(true);
            movementSpeed = attackSpeed;
        }
        else
        {
            currentBehavior = enemyBehavior.Idle;
            light.gameObject.SetActive(false);
            movementSpeed = normalSpeed;
        }
    }

    private void Attack()
    {
        PointBroom();
        anim.LookAt(PlayerMovement.playerPosition);

        if (attackBrooming)
        {
            currentTime += Time.deltaTime;
            timeSinceLastBrush += Time.deltaTime;

            if (timeSinceLastBrush > 0.3f)
            {
                timeSinceLastBrush = 0;
                SpawnParticles();
            }


            Vector2 dir = PlayerMovement.playerPosition - transform.position;

            Move(dir);

            if (currentTime > broomingTime)
            {
                timeSinceLastBrush = 0;
                broomAnimator.SetBool("Attacking", false);
                light.SetBool("Fast", false);
                currentTime = 0;
                attackBrooming = false;
            }
        }
        else
        {
            currentTime += Time.deltaTime;

            if(currentTime > waitTime)
            {
                broomAnimator.SetBool("Attacking", true);
                light.SetBool("Fast", true);
                currentTime = 0;
                attackBrooming = true;
            }
        }
    }

    private void SpawnParticles()
    {

        Vector3 dir = PlayerMovement.playerPosition - transform.position;

        for (int i = 0; i < 8; i++)
        {
            float ran = Random.Range(-25, 25);
            Vector3 newDir = Quaternion.Euler(0, 0, ran) * dir;
            BroomParticle brushParticles = Instantiate(brushParticlesPrefab, particleSpawn.position, Quaternion.identity).gameObject.GetComponent<BroomParticle>();
            brushParticles.SetDirection(newDir);
        }
    }

    private void PointBroom()
    {
        anim.UpdateGunRotatation(PlayerMovement.playerPosition);
    }

    public override void Dead()
    {
        broomHolder.gameObject.SetActive(false);
        light.enabled = false;
        base.Dead();
    }
}
