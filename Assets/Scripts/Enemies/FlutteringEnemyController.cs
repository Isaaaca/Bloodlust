using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutteringEnemyController : CharacterMovementController, ICharacter
{
    [Header("Character Settings")]
    public float range = 10;
    public float pauseTime = 2;
    [SerializeField] private Meter health = null;


    private Animator animator;
    private float timer;
    private Vector2 targetPoint;
    private Vector2 origin;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        targetPoint = origin = rb2d.position;
        timer = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (timer <= 0)
        {
            Vector2 dir = targetPoint - rb2d.position;
            if (dir.magnitude < 0.1)
            {
                targetPoint = GetNextDestination();
                velocity = Vector2.zero;
                HoriMove(0);
                VertMove(0);
                timer = pauseTime;
            }
            else
            {
                dir = dir.normalized;
                HoriMove(dir.x);
                VertMove(dir.y);
            }
        }
        timer -= Time.deltaTime;
        base.Update();
    }

    private Vector2 GetNextDestination()
    {
        float a = Random.value * 2 * Mathf.PI;
        float r = range * Mathf.Sqrt(Random.value);

        //Cartesian coordinates
        float x = r * Mathf.Cos(a);
        float y = r * Mathf.Sin(a);
        return origin + new Vector2(x, y);
    }

    public void TakeDamage(float dmg)
    {
        if (health.Get() > 0)
        {
            health.Set(Mathf.Clamp(health.Get() - dmg, 0, health.GetMax()));
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.localPosition, range);
        Gizmos.DrawIcon(targetPoint, "target");
    }

    public Meter GetHealth()
    {
        return health;
    }
}
