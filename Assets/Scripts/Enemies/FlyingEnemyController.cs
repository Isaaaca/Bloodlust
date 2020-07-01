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
    [SerializeField] private Meter health = null;
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float deaggroRange = 0;
    [SerializeField] private float lungeRange = 0;
    [SerializeField] private float windUpTime = 0;
    [SerializeField] private float diveDuration = 0;
    public Vector2[] wayPoints ;
    public bool loop;

    private int nextWaypoint = 1;
    private int step = 1;
    private float windUpTimer = 0;
    private Animator animator;
    private GameObject player;
    private bool aggro = false;
    private bool windingUp = false;
    private Vector2 lungeDir= Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        wayPoints[0] = transform.position;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        Vector2 vectToPlayer = (Vector2)player.transform.position - rb2d.position;
        float distance = vectToPlayer.magnitude;
        if (!arcing)
        {
            if (distance < lungeRange && vectToPlayer.y<=0)
            {
                if (!windingUp)
                {
                    windingUp = true;
                    lungeDir = vectToPlayer;
                }
            }
            else if (distance < aggroRange)
            {
                //chase
                aggro = true;
            }
            else if (distance > deaggroRange)
            {
                aggro = false;
            }

            if (windingUp)
            {
                windUpTimer += Time.deltaTime;
                if (windUpTimer >= windUpTime)
                {
                    Arc(Vector2.down, lungeDir.y, lungeDir.x * 2, diveDuration, true, "linear", "sine");
                    windUpTimer = 0;
                    windingUp = false;
                }
                else
                { 
                    velocity = Vector2.zero;
                    HoriMove(0);
                    VertMove(0);
                }
            }
            else
            {
                Vector2 dir;
                if (aggro)
                {
                    dir = (Vector2)player.transform.position - rb2d.position;
                    dir = dir.normalized;
                }
                //move to next waypoint
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
                        else if (0 > nextWaypoint || nextWaypoint >= wayPoints.Length)
                        {
                            step *= -1;
                            nextWaypoint = (nextWaypoint + 2 * step);

                        }
                    }
                    dir = dir.normalized;
                }

                HoriMove(dir.x);
                VertMove(dir.y);
            }
        }
        
        
        base.Update();
    }

    public void TakeDamage(float dmg)
    {
        if (health.Get() > 0)
        {
            health.Modify(-dmg);
            animator.SetTrigger("Hurt");
            if (health.Get() == 0)
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
        Gizmos.DrawWireSphere(transform.localPosition, lungeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.localPosition, aggroRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.localPosition, deaggroRange);
    }

    public Meter GetHealth()
    {
        throw new System.NotImplementedException();
    }
}
