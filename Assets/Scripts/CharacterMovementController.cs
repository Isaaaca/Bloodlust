using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : PhysicsObject
{

    public float groundSpeed = 7f;
    public float airSpeed = 8f;
    public float jumpTakeOffSpeed = 15f;
    public float smallJumpDuration = 0.01f;
    public float fallingGravityMultiplier = 1.3f;
    public float cancelJumpGravity = 3;
    public float backHopDistance = 2.5f;
    public float backHopDuration = 0.32f;

    protected SpriteRenderer spriteRenderer;
    protected bool backHopping;
    protected bool jumped;
    protected bool facingRight=false;
    protected float backHopAirtime;
    protected float smallJumpAirtime;

    private float xMovement;
    private bool jumpInput = false;
    private bool releasedJumpInput = true;
    private bool backHopInput = false;



    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        backHopAirtime = backHopDuration;
        smallJumpAirtime =0;
    }

    public bool Jump()
    {
        if (grounded)
        {
            jumpInput = true;
            return true;
        }

        return false;
    }

    public void ReleaseJump()
    {
        releasedJumpInput = true;
        jumpInput = false;
    }

    public void Walk(float xVelocity)
    {
        xMovement = xVelocity;
    }

    public bool BackHop()
    {
        if (grounded)
        {
            backHopInput = true;
            return true;
        }
        return false;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        bool newFacingDir = facingRight;

        if (grounded)
        {
            if (jumpInput && releasedJumpInput)
            {
                groundNormal = Vector2.up;
                jumped = true;
                releasedJumpInput = false;
                gravityModifier = baseGravityModifier;
                velocity.y = jumpTakeOffSpeed;
            }
            if (backHopInput)
            {
                backHopping = true;
            }
            //only reset when landing or standing still. Else might get reset mid-jump when still registered as grounded.
            if (velocity.y <=0f)
            {
                smallJumpAirtime = smallJumpDuration;
                jumped = false;
            }

        }
        else
        {
            if (jumped)
            {

                if (smallJumpAirtime > 0 && velocity.y > 0)
                {
                    velocity.y = jumpTakeOffSpeed;
                    smallJumpAirtime -= Time.deltaTime;
                }
                else if (releasedJumpInput)
                {
                    gravityModifier = baseGravityModifier * cancelJumpGravity;
                }
            }
            if (velocity.y < 0)
            {
                gravityModifier = baseGravityModifier * fallingGravityMultiplier;
            }
        }


        if (backHopping)
        {
            move.x = (facingRight ? -1 : 1) * backHopDistance / backHopDuration * (Mathf.Sin(backHopAirtime / backHopDuration * Mathf.PI - Mathf.PI / 2) + 1);
            backHopAirtime = backHopAirtime - Time.deltaTime;
            if (backHopAirtime <= 0)
            {
                backHopInput = false;
                backHopping = false;
                backHopAirtime = backHopDuration;
                move.x = 0;
            }
        }
        else
        {
            move.x = xMovement * (grounded ? groundSpeed : airSpeed);
            newFacingDir = (move.x == 0 ? facingRight : move.x > 0.00f);
        }


        if (newFacingDir!=facingRight)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            facingRight = newFacingDir;
        }

        targetVelocity = move;
    }
}
