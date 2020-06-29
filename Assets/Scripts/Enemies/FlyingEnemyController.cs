using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : CharacterMovementController, ICharacter
{
    public enum State
    {
        patrol,
        chase
    }

    [HideInInspector] public State state = State.patrol;

    [Header("Character Settings")]
    public float health = 100;
    public float maxHealth = 100;
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float deaggroRange = 0;
    public Vector2[] wayPoints ;
    public bool loop;

    private int nextWaypoint = 1;
    private int step = 1;
    private Animator animator;
    private GameObject player;
    private bool aggro = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        wayPoints[0] = transform.position;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected override void  Update()
    {
        float distance = ((Vector2)player.transform.position - rb2d.position).magnitude;
        if (distance < aggroRange)
        {
            //chase
            aggro = true;
        }
        else if(distance> deaggroRange)
        {
            aggro = false;
        }
        Vector2 dir;
        if (aggro)
        {
            dir = (Vector2)player.transform.position - rb2d.position;
        }
        else
        {
            dir = (wayPoints[nextWaypoint] - rb2d.position);
            if (dir.magnitude < 0.1) 
            {
                nextWaypoint = (nextWaypoint + step); 
                if (loop) 
                {
                    nextWaypoint = nextWaypoint % wayPoints.Length;
                } 
                else if(0>nextWaypoint || nextWaypoint >= wayPoints.Length)
                {
                    step *= -1;
                    nextWaypoint = (nextWaypoint + 2*step); 

                }
            }
        }
        dir = dir.normalized;
        HoriMove(dir.x);
        VertMove(dir.y);
        base.Update();
    }

    public void TakeDamage(float dmg)
    {
        if (health > 0)
        {
            health = Mathf.Clamp(health - dmg, 0, maxHealth);
            animator.SetTrigger("Hurt");
            if (health == 0)
            {
                animator.SetBool("Dead", true);
            }
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.localPosition, aggroRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.localPosition, deaggroRange);
    }
}
