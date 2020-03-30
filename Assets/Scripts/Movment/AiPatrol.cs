using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrol : MonoBehaviour
{

    [SerializeField] private GroundedEnemy controller = null;
    [SerializeField] private Transform groundDetection = null;
    private bool movingRight = true;
    private int layer_mask;

    private void Start()
    {
        layer_mask = LayerMask.GetMask("Obstacle");
    }
    private void Update()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down,1f,layer_mask);
        Collider2D wallInfo = Physics2D.OverlapPoint(groundDetection.position,layer_mask);
        if (groundInfo.collider == false || wallInfo!=false)
        {
            movingRight = !movingRight;
        }
        controller.HoriMove(movingRight ? 1: -1);
    }
}