using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterMovementController
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", Jump());
        }
        if (Input.GetButtonUp("Jump"))ReleaseJump();
        if (Input.GetKeyDown(KeyCode.E)) BackHop();
        Walk(Input.GetAxis("Horizontal"));
        base.Update();
        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Running", Mathf.Abs(velocity.x)>0);
        animator.SetFloat("yVelocity", velocity.y);
    }
}
