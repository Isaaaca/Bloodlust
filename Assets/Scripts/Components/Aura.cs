using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Aura : MonoBehaviour
{
    public float strength;
    public float radius;
    private Light2D glow;
    private CircleCollider2D auraCircle;

    private void Start()
    {
        glow = GetComponent<Light2D>();
        glow.pointLightOuterRadius = radius;
        glow.pointLightInnerRadius = radius*0.2f;
        auraCircle = GetComponent<CircleCollider2D>();
        auraCircle.radius = radius;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        float distance = Vector3.Distance(other.transform.position,transform.position);
        float resultantStrength = strength / (distance / radius);
        PlayerController player = other.GetComponent<PlayerController>();
        if(player!=null)
            player.ModifyLust(resultantStrength *Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
