using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float drag = 0f;
    [SerializeField] protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    [SerializeField] protected float gravityModifier;
    [SerializeField] protected Vector2 initialVelocity;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        velocity = initialVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime - velocity.normalized * drag;

        Vector2 deltaposition = velocity * Time.fixedDeltaTime;

        rb2d.position += deltaposition;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)initialVelocity/10f);
    }
}
