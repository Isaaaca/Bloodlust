using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [Header("Character Settings")]
    public Vector3 attackRangeCenter;
    public float attackRange;
    public float backHopDistance = 2.5f;
    public float backHopDuration = 0.32f;
    public float dashDistance = 2.5f;
    public float dashDuration = 0.32f;
    public float invulnerabilityDuration = 1f;
    [SerializeField] private Meter lust = null;

    [Header("Child Scripts")]
    [SerializeField] private QuickTimeEvent qte = null;
    [SerializeField] private Hurtbox sword = null;

    private bool playerInControl;
    private bool dashed = false;
    private float knockbackTime = 0.4f;
    private float invulTimer = 0f;



    protected override void Start()
    {
        base.Start();
        InvokeRepeating("UpdatePlayerControl", 2.0f, 2f);
    }

    private void Update()
    {
        if(controller.IsGrounded() && !controller.IsDashing())
        {
            dashed = false;
        }
        if (!playerInControl)
        {
            if (qte.state != QuickTimeEvent.State.Neutral)
            {
                if (qte.state == QuickTimeEvent.State.Failed)
                    Attack(10f);
                else if (qte.state == QuickTimeEvent.State.PassedAggro)
                {
                    Attack(30f);
                    Debug.Log("Aggro");
                }

                qte.enabled = false;
            }
            playerInControl = !qte.enabled;
        }
        else 
        {
            GetInputs();
        }


        if (invulTimer > 0)
        {
            invulTimer -= Time.deltaTime;
            sprite.enabled = !sprite.enabled;
            if (invulTimer <= 0)
                sprite.enabled = true;
        }

        UpdateAnimationState();

    }

    private void GetInputs()
    {
        if (!(invulTimer > (invulnerabilityDuration - knockbackTime)))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
                && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                controller.HoriMove(Input.GetAxis("Horizontal"));
                if (Input.GetButtonDown("Jump"))
                {
                    if (controller.Jump())
                        animator.SetTrigger("Jump");
                }
                if (controller.IsGrounded() && Input.GetKeyDown(KeyCode.E))
                {
                    Vector2 dir = controller.IsFacingRight() ? Vector2.left : Vector2.right;
                    if (controller.Dash(dir, backHopDistance, backHopDuration, false, "Sine"))
                    {
                        animator.SetTrigger("Dash");
                    }
                }
                if (!dashed && Input.GetKeyDown(KeyCode.O))
                {
                    Vector2 dir = controller.IsFacingRight() ? Vector2.right : Vector2.left;
                    if (controller.Dash(dir, dashDistance, dashDuration, true, "Sine"))
                    {
                        dashed = true;
                        animator.SetTrigger("Dash");
                    }
                }
            }
            else if (!controller.IsGrounded())
            {
                controller.HoriMove(Input.GetAxis("Horizontal"));
            }
            else
            {
                controller.HoriMove(0);
            }
        }
        if (Input.GetButtonUp("Jump")) controller.ReleaseJump();
        if (Input.GetKeyDown(KeyCode.T)) Attack(10f);
    }

    private void UpdateAnimationState()
    {
        animator.SetBool("Grounded", controller.IsGrounded());
        animator.SetBool("Running", Mathf.Abs(controller.GetVelocity().x)>0);
        animator.SetFloat("yVelocity", controller.GetVelocity().y);
    }

    private void UpdatePlayerControl()
    {
        if (Random.value <= lust.GetNormalised())
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.localPosition + attackRangeCenter, attackRange, LayerMask.GetMask("Enemy"));
            if (hitColliders.Length > 0)
            {
                float minDis = float.MaxValue;
                float distance;
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    //TODO: get closest
                    distance = hitColliders[i].transform.position.x - transform.position.x;
                    if (Mathf.Abs(distance) < Mathf.Abs(minDis)) minDis = distance;


                }
                controller.HoriMove(0f);
                if ((minDis > 0) != controller.IsFacingRight())
                    controller.Turn();
                qte.enabled = true;
                playerInControl = false;
            }
        }
    }


    public void Attack(float dmg)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")||animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            animator.SetTrigger("Attack2");
        }
        else
        {
            animator.SetTrigger("Attack");
        }
        sword.SetDamage(dmg);
    }

    public override void TakeDamage(float dmg, Vector2 source)
    {

        if (health.Get() > 0 && invulTimer<=0)
        {
            invulTimer = invulnerabilityDuration;
            health.Modify(-dmg);
            controller.HoriMove(0);
            controller.VertMove(0);
            Vector2 knockbackDir = (rb2d.position - source);
            if (knockbackDir.x < 0 != controller.GetFacingRight()) controller.Turn();
            controller.Arc(Vector2.up, 0.4f, Mathf.Sign(knockbackDir.x) * -1f, 0.5f, false, "easein", "linear");
            animator.SetTrigger("Hurt");
            if(health.Get() == 0)
            {
                animator.SetBool("Dead",true);
            }
        }
    }

    public void ModifyLust(float amt)
    {
        lust.Modify(amt);
    }

    public Meter GetLust()
    {
        return lust;
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.localPosition + attackRangeCenter, attackRange);
    }
}
