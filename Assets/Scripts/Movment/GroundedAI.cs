using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class GroundedAI : MonoBehaviour
{
    public Transform target;
    public float nextWPDistance = 3f;
    CharacterMovementController controller;

    Path path;
    int currWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterMovementController>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currWaypoint = 0;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (path == null) return;

        if(currWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            controller.HoriMove(0);
            controller.VertMove(0);
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currWaypoint] - rb2d.position).normalized;

        controller.HoriMove(direction.x);
        if(direction.y >0)
            controller.Jump();

        if (Vector2.Distance(rb2d.position, path.vectorPath[currWaypoint]) < nextWPDistance) currWaypoint++;
        
    }
}
