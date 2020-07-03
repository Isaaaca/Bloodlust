using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private float damage = 10f;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    
        Character other = col.GetComponent<Character>();
        if (other != null) other.TakeDamage(damage);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        Character other = col.GetComponent<Character>();
        if (other != null) other.TakeDamage(damage);
    }
}
