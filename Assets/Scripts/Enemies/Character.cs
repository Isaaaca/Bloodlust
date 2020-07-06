using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Settings")]
    public Meter health;

    protected CharacterMovementController controller;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rb2d;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        controller = GetComponent<CharacterMovementController>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

 

    public virtual void TakeDamage(float dmg, Vector2 source)
    {
        if (health.Get() > 0)
        {
            health.Modify(-dmg);
            animator.SetTrigger("Hurt");
            if (health.Get() == 0)
            {
                animator.SetBool("Dead", true);
            }
        }
    }

    public virtual void OnDeath()
    {
        animator.SetBool("Dead", true);
        Destroy(gameObject);
    }

    public Meter GetHealth()
    {
        return health;
    }
}
