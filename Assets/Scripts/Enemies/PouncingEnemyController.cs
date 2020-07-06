using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouncingEnemyController : Character
{
    [SerializeField] private Transform groundDetection = null;
    [SerializeField] private bool goOnSlope = false;
    private bool movingRight = true;
    private int layer_mask;

    protected override void Start()
    {
        base.Start();
        layer_mask = LayerMask.GetMask("Obstacle");
    }
    private void Update()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.3f, layer_mask);
        Collider2D wallInfo = Physics2D.OverlapPoint(groundDetection.position, layer_mask);
        if (groundInfo.collider == false || wallInfo != false || (!goOnSlope && groundInfo.normal.y != 1))
        {
            movingRight = !movingRight;
            controller.Halt();
        }
        controller.HoriMove(movingRight ? 1 : -1);
    }
}
