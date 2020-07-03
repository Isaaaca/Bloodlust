using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutteringEnemyController : Character
{
    [Header("Character Settings")]
    public float range = 10;
    public float pauseTime = 2;


    private float timer;
    private Vector2 targetPoint;
    private Vector2 origin;
    // Start is called before the first frame update
    protected override void  Start()
    {
        base.Start();
        targetPoint = origin = rb2d.position;
        timer = 0;
    }

    // Update is called once per frame
    private void Update()
    {

        if (timer <= 0)
        {
            Vector2 dir = targetPoint - rb2d.position;
            if (dir.magnitude < 0.1)
            {
                targetPoint = GetNextDestination();
                controller.SetVelocity(Vector2.zero);
                controller.HoriMove(0);
                controller.VertMove(0);
                timer = pauseTime;
            }
            else
            {
                dir = dir.normalized;
                controller.HoriMove(dir.x);
                controller.VertMove(dir.y);
            }
        }
        timer -= Time.deltaTime;
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

  
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.localPosition, range);
        Gizmos.DrawIcon(targetPoint, "target");
    }

}
