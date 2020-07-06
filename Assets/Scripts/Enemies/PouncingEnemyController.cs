using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouncingEnemyController : Character
{
    [Header("Character Settings")]
    [SerializeField] private float aggroRange = 0;
    [SerializeField] private float lungeDistance = 0;
    [SerializeField] private float lungeDuration = 0;
    [SerializeField] private float windUpTime = 0;
    [SerializeField] private float coolDownTime = 0;
    [SerializeField] private Transform groundDetection = null;
    [SerializeField] private bool goOnSlope = false;
    private int layer_mask;
    private GameObject player;
    private float windUpTimer = 0;
    private bool windingUp = false;
    private float coolDownTimer = 0;
    private bool coolDown = false;
    private Vector2 lungeDir = Vector2.right;

    protected override void Start()
    {
        base.Start();
        layer_mask = LayerMask.GetMask("Obstacle");
        player = GameObject.Find("Player");
    }
    private void Update()
    {
       
       

        Vector2 vectToPlayer = (Vector2)player.transform.position - rb2d.position;
        float distance = vectToPlayer.magnitude;
        bool isFacingPlayer = vectToPlayer.x > 0 ? controller.IsFacingRight() : !controller.IsFacingRight();
        if (!controller.IsDashing())
        {
            if (distance < aggroRange && isFacingPlayer)
            {
                if (!windingUp && !coolDown)
                {
                    windingUp = true;
                    lungeDir = Vector2.right * Mathf.Sign(vectToPlayer.x);
                }
            }

            if (windingUp)
            {
                windUpTimer += Time.deltaTime;
                if (windUpTimer >= windUpTime)
                {
                    controller.Dash(lungeDir, lungeDistance, lungeDuration, true, "easeIn");
                    windUpTimer = 0;
                    windingUp = false;
                    coolDown = true;
                }
                else
                {
                    controller.SetVelocity(Vector2.zero);
                    controller.HoriMove(0);
                    controller.VertMove(0);
                }
            }
            else if (coolDown)
            {
                coolDownTimer += Time.deltaTime;
                if (coolDownTimer >= coolDownTime)
                {
                    coolDownTimer = 0;
                    coolDown = false;
                }
            }  


        }
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.3f, layer_mask);
        Collider2D wallInfo = Physics2D.OverlapPoint(groundDetection.position, layer_mask);
        if (groundInfo.collider == false || wallInfo != false || (!goOnSlope && groundInfo.normal.y != 1))
        {
            controller.Halt();
            if(!(distance < aggroRange && isFacingPlayer))
                controller.Turn();
        }
        //Patrol normally if not Halting
        else if (!controller.IsDashing() && !coolDown && !windingUp)
        {
            controller.HoriMove(controller.IsFacingRight() ? 1 : -1);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.localPosition, lungeDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.localPosition, aggroRange);
    }
}
