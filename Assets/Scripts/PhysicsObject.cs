using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [Header("Physics Settings")]
    public float minGroundNormalY = 0.65f;
    public float baseGravityModifier = 1f;

    protected float gravityModifier;
    protected bool grounded;
    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected Vector2 velocity; 
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gravityModifier = baseGravityModifier;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();

    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;
        velocity.x = targetVelocity.x;
        if (targetVelocity.y != 0) velocity.y = targetVelocity.y;

        grounded = false;

        Vector2 deltaposition = velocity * Time.fixedDeltaTime;

        Vector2 move = Vector2.up * deltaposition.y;
        Movement(move, true);

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        move = (grounded? moveAlongGround:Vector2.right)* deltaposition.x ;
        Movement(move, false);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i<count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection<0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }
        rb2d.position += move.normalized * distance;
    }


}
