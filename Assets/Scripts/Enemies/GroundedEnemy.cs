using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemy : CharacterMovementController, ICharacter
{
    [Header("Character Settings")]
    public Meter health;
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
        if (health.Get() > 0)
        {
            health.Set(Mathf.Clamp(health.Get() - dmg, 0, health.GetMax()));
            animator.SetTrigger("Hurt");
            if (health.Get() == 0)
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

    public Meter GetHealth()
    {
        return health;
    }
}
