using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRoot;
    [SerializeField]
    private Sprite forward;
    [SerializeField]
    private Sprite backward;
    [SerializeField]
    private SpriteRenderer gunSprite;

    private Animator animator;

    private float startScale = 1;

    private void Start()
    {
        Init();
    }

    protected void Init()
    {
        startScale = spriteRoot.transform.localScale.x;
        animator = GetComponent<Animator>();
    }

    public void LookAt(Vector3 position)
    {
        Vector3 scale = spriteRoot.transform.localScale;
        float direction = Mathf.Sign(position.x - transform.root.position.x);

        if (position.y - transform.root.position.y < 0)
        {
            spriteRoot.sprite = forward;
        }
        else
        {
            spriteRoot.sprite = backward;
        }


        scale.x = direction * startScale;
        spriteRoot.transform.localScale = scale;
    }

    public void SetWalking(bool walking)
    {
        animator.SetBool("Walking", walking);
    }

    public void SetDead()
    {
        animator.SetBool("Dead", true);
    }
}
