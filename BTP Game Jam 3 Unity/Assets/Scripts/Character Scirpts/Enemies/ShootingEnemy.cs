using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootingEnemy : EnemyBehaviorScript
{
    [SerializeField]
    private GunScript gun;
    [SerializeField]
    private LayerMask lookMask;

    /// <summary>
    /// Under this distance the enemy will move backwards.
    /// </summary>
    private float toCloseDistance = 4.5f;

    private float closeRange = 7.5f;


    /// <summary>
    /// The enemy is able to to track the player and move towards him.
    /// </summary>
    private float inRangeDistance = 12;


    private float gunCooldown = 1f;
    private float currentGunCooldown = 1;


    [SerializeField]
    private bool canSeePlayer = false;

    private void Start()
    {
        base.Init();
    }

    void Update()
    {

        anim.SetWalking(false);
        CheckBehavior();
        canSeePlayer = CheckIfPlayerIsSeen();

        switch (currentBehavior)
        {
            case enemyBehavior.Attack:
                ShootingRoutine();
                break;
        }
    }

    private bool CheckIfPlayerIsSeen()
    {
        Vector3 dir = PlayerMovement.playerPosition - transform.position;
        if (dir.magnitude > inRangeDistance)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 60, lookMask);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, dir.normalized * ((Vector2)transform.position - hit.point).magnitude, Color.green, 0.05f);
            if((hit.point - (Vector2)PlayerMovement.playerPosition).magnitude < 1)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Look what the right behavior is.
    /// </summary>
    private void CheckBehavior()
    {
        if ((PlayerMovement.playerPosition - transform.position).sqrMagnitude < inRangeDistance * inRangeDistance && canSeePlayer)
        {
            if(currentBehavior == enemyBehavior.Idle)
                SpottedPlayer();
            currentBehavior = enemyBehavior.Attack;
        }
        else
        {
            currentBehavior = enemyBehavior.Idle;
        }
    }

    /// <summary>
    /// Routine executing while in shooting state.
    /// </summary>
    private void ShootingRoutine()
    {
        float playerDistance = (PlayerMovement.playerPosition - transform.position).sqrMagnitude;

        if (playerDistance < inRangeDistance * inRangeDistance)
        {
            // Player is in shooting range.
            if (currentGunCooldown < gunCooldown)
            {
                currentGunCooldown += Time.deltaTime;
            }
            else
            {
                currentGunCooldown = 0;
                gun.Shoot(PlayerMovement.playerPosition, 6f);
            }

            // Check if player is to close.
            if (playerDistance < toCloseDistance * toCloseDistance)
            {
                // Move away from the player.
                Vector2 dir = PlayerMovement.playerPosition - transform.position;
                Move(-dir);
            }
        }

        if(playerDistance > closeRange * closeRange)
        {
            // Player is outside shooting range, so approach him.
            Vector2 dir = PlayerMovement.playerPosition - transform.position;
            Move(dir);
        }

        anim.UpdateGunRotatation(PlayerMovement.playerPosition);
        anim.LookAt(PlayerMovement.playerPosition);
    }

    public override void Dead()
    {
        gun.gameObject.SetActive(false);
        base.Dead();
    }
}
