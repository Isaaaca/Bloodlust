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
    public Vector3 attackRangeCenter;
    public float attackRange;

    [Header("Child Scripts")]
    [SerializeField] private QuickTimeEvent qte;
    [SerializeField] private Hurtbox sword;

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
                Time.timeScale = 1;
            }
            playerInControl = !qte.enabled;
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
        if (Input.GetKeyDown(KeyCode.T)) Attack(10f);
    }

    private void UpdateAnimationState()
    {
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Running", Mathf.Abs(velocity.x)>0);
        animator.SetFloat("yVelocity", velocity.y);
    }

    private void UpdatePlayerControl()
    {
        if (Random.value <= health.GetNormalised())
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
                if ((minDis > 0)!= facingRight) Turn();
                qte.facingRight = facingRight;
                qte.enabled = true;
                Time.timeScale = 0;
                playerInControl = false;
            }
        }
    }


    public void Attack(float dmg)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
        }
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.localPosition + attackRangeCenter, attackRange);
    }
}
