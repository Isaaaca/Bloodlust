using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemy : CharacterMovementController, ICharacter
{
    [Header("Character Settings")]
    public float health = 100;
    public float maxHealth =100;
    public float lust;
    public float maxLust;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

 

    public void TakeDamage(float dmg)
    {
        if (health > 0)
        {
            health = Mathf.Clamp(health - dmg, 0, maxHealth);
            animator.SetTrigger("Hurt");
            if (health == 0)
            {
                OnDeath();
            }
        }
    }

    public void OnDeath()
    {
        animator.SetBool("Dead", true);
        Destroy(gameObject);
    }
}
