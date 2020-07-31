using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public static event Action<Character> OnCharacterDeath = (character) => { };

    [Header("Generic Character Settings")]
    public Meter health;

    protected CharacterMovementController controller;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rb2d;
    protected Vector2 initialPosition;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        controller = GetComponent<CharacterMovementController>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        initialPosition = rb2d.position;
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
        Character.OnCharacterDeath(this);
        gameObject.SetActive(false);
    }

    public Meter GetHealth()
    {
        return health;
    }

    public virtual void Respawn()
    {
        animator.SetBool("Dead", false);
        rb2d.position = initialPosition;
        health.Set(health.GetMax());
    }
}
