using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyBehavior { Idle, Attack, Dead, Wait}

/// <summary>
/// Parent class for all enemy behaviors.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBehaviorScript : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed = 4;
    [SerializeField]
    protected GunCharacterAnimator anim;
    [SerializeField]
    private GameObject playerSpotted;
    [SerializeField]
    private AudioClip spottedClip;

    protected enemyBehavior currentBehavior;

    protected Rigidbody2D rigi;

    protected void Init()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    protected void Move(Vector2 dir)
    {
        anim.SetWalking(true);
        Vector2 vel = rigi.velocity;
        vel += dir.normalized * movementSpeed * Time.deltaTime;
        rigi.velocity = vel;
    }

    public virtual void Dead()
    {
        currentBehavior = enemyBehavior.Dead;
        if(anim != null)
            anim.SetDead();
        this.enabled = false;
    }

    protected void ChangeBehavior(enemyBehavior newBehavior)
    {
        // TODO: implement on central method for changes.
    }

    protected void SpottedPlayer()
    {
        GlobalAudioSource.PlaySoundEffect(spottedClip);

        // TODO: pool
        Destroy(Instantiate(playerSpotted, transform.position, Quaternion.identity), 1f);
    }
}
