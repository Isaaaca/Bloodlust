using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterMovementController, ICharacter
{
    [Header("Character Settings")]
    public Vector3 attackRangeCenter;
    public float attackRange;

    [Header("Child Scripts")]
    [SerializeField] private QuickTimeEvent qte = null;
    [SerializeField] private Hurtbox sword = null;
    [SerializeField] private HealthMeter health = null;
    [SerializeField] private LustMeter lust = null;

    private Animator animator;
    private bool playerInControl;



    void Start()
    {
        animator = GetComponent<Animator>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        InvokeRepeating("UpdatePlayerControl", 2.0f, 2f);
    }

    protected override void Update()
    {
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
            GetInputs();
        base.Update();

        UpdateAnimationState();

    }

    private void GetInputs()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            &&!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            HoriMove(Input.GetAxis("Horizontal"));
            if (Input.GetButtonDown("Jump"))
            {
                if (Jump())
                    animator.SetTrigger("Jump");
            }
            if (Input.GetKeyDown(KeyCode.E)) BackHop();
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (Dash())
                    animator.SetTrigger("Dash");
            }
        }
        else if (!grounded)
        {
            HoriMove(Input.GetAxis("Horizontal"));
        }
        else
        {
            HoriMove(0);
        }
        if (Input.GetButtonUp("Jump")) ReleaseJump();
        if (Input.GetKeyDown(KeyCode.T)) Attack(10f);
    }

    private void UpdateAnimationState()
    {
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Running", Mathf.Abs(targetVelocity.x)>0);
        animator.SetFloat("yVelocity", velocity.y);
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
                HoriMove(0f);
                if ((minDis > 0) != facingRight)
                    Turn();
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

    public void TakeDamage(float dmg)
    {

        if (health.Get() > 0)
        {
            health.Modify(-dmg);
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.localPosition + attackRangeCenter, attackRange);
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}
