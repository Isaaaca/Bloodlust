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
        if (grounded && Mathf.Abs(velocity.x) > Mathf.Epsilon)
            animator.SetInteger("AnimState", 2);
        else
            animator.SetInteger("AnimState", 0);
        animator.SetBool("Grounded", grounded);

    }
}
