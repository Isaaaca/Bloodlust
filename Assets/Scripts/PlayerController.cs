using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterMovementController, ICharacter
{
    [Header("Character Settings")]
    public float health;
    public float maxHealth;
    public float lust;
    public float maxLust;

    private Animator animator;



    void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        GetInputs();
        base.Update();
        UpdateAnimationState();

    }

    private void GetInputs()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (Jump())
                animator.SetTrigger("Jump");
        }
        if (Input.GetButtonUp("Jump")) ReleaseJump();
        if (Input.GetKeyDown(KeyCode.E)) BackHop();
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (Dash())
                animator.SetTrigger("Dash");

        }
        VertMove(Input.GetAxis("Vertical"));
        HoriMove(Input.GetAxis("Horizontal"));
        if (Input.GetKeyDown(KeyCode.T)) TakeDamage(10f);
    }

    private void UpdateAnimationState()
    {
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Running", Mathf.Abs(velocity.x)>0);
        animator.SetFloat("yVelocity", velocity.y);
    }

    public void TakeDamage(float dmg)
    {
        if (health > 0)
        {
            health = Mathf.Clamp(health-dmg,0,maxHealth);
            animator.SetTrigger("Hurt");
            if(health == 0)
            {
                animator.SetBool("Dead",true);
            }
        }
    }
}
