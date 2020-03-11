using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : PhysicsObject
{
    [Header("Movement Settings")]
    public float groundSpeed = 7f;
    public float airSpeed = 8f;
    public float jumpTakeOffSpeed = 15f;
    public float smallJumpDuration = 0.01f;
    public float fallingGravityMultiplier = 1.3f;
    public float cancelJumpGravity = 3;
    public float backHopDistance = 2.5f;
    public float backHopDuration = 0.32f;
    public float dashDistance = 2.5f;
    public float dashDuration = 0.32f;

    protected SpriteRenderer spriteRenderer;
    protected bool backHopping;
    protected bool dashing;
    protected bool jumped = false;
    protected bool dashed = false;
    protected bool facingRight=false;
    protected float backHopAirtime;
    protected float dashAirtime;
    protected float smallJumpAirtime;

    private float xMovement;
    private float yMovement;
    private bool jumpInput = false;
    private bool releasedJumpInput = true;
    private bool backHopInput = false;
    private bool dashInput = false;



    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        backHopAirtime = backHopDuration;
        dashAirtime = dashDuration;
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

    public void HoriMove(float xVelocity)
    {
        xMovement = xVelocity;
    }

    public void VertMove(float yVelocity)
    {
        yMovement = yVelocity;
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
    
    public bool Dash()
    {
        if (!dashed)
        {
            dashInput = true;
            dashed = true;
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
                dashed = false;
            }

        }
        else
        {
            if (jumped)
            {

                if (smallJumpAirtime > 0 && velocity.y >= 0)
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

        if (dashInput)
        {
            dashing = true;
            if((facingRight? groundNormal.x>0 : groundNormal.x < 0)|| !grounded)
            {
                groundNormal = Vector2.up;
                velocity.y = 0;
                gravityModifier = 0;
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
        else if (dashing)
        {
            move.x = (facingRight ? 1 : -1) * dashDistance / dashDuration * (Mathf.Sin(dashAirtime / dashDuration * Mathf.PI - Mathf.PI / 2) + 1);
            dashAirtime = dashAirtime - Time.deltaTime;
            if (dashAirtime <= 0)
            {
                dashInput = false;
                dashing = false;
                dashAirtime = dashDuration;
                move.x = 0;
                gravityModifier = baseGravityModifier;
            }
        } 
        else
        {
            move.x = xMovement * (grounded ? groundSpeed : airSpeed);
            move.y = yMovement * airSpeed;
            newFacingDir = (move.x == 0 ? facingRight : move.x > 0.00f);
        }


        if (newFacingDir!=facingRight)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            facingRight = newFacingDir;

            foreach (Transform child in transform)
            {
                child.localPosition = new Vector3(-child.localPosition.x,child.localPosition.y,child.localPosition.z);

            }
        }

        targetVelocity = move;
    }
}
