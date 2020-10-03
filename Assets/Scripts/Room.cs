﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static event Action<Room> OnEnterRoom = (room) => { };

    [SerializeField] private float topBorder = 1;
    [SerializeField] private float bottomBorder = 1;
    [SerializeField] private float leftBorder = 1;
    [SerializeField] private float rightBorder = 1;
    private List<GameObject> childObjects = new List<GameObject>();
    private PolygonCollider2D col2D;
    private bool roomActive = false;


    private void Awake()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        Bounds bounds = colliders[0].bounds;
        foreach (Collider2D col in colliders)
        {
            bounds.Encapsulate(col.bounds);
        }
        bounds.center += Vector3.up * (topBorder / 2) + Vector3.up * (bottomBorder / 2)  
            + Vector3.left * (rightBorder / 2) + Vector3.right * (leftBorder / 2);
        bounds.center -= transform.position;
        bounds.size += Vector3.up * (topBorder-bottomBorder);
        bounds.size -= Vector3.right*(leftBorder+rightBorder);

        col2D = gameObject.AddComponent<PolygonCollider2D>();

        col2D.isTrigger = true;
        col2D.pathCount = 1;
        col2D.points = new Vector2[4]
            {
                new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y),
                new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y),
                new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y),
                new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y),
            };

        GetRoomChildren();
        DeactivateChildren();

        Room.OnEnterRoom += HandleEnterRoomEvent;
        
    }

    private void OnDestroy()
    {
        Room.OnEnterRoom -= HandleEnterRoomEvent;

    }
    private void HandleEnterRoomEvent(Room enteredroom)
    {
        if (roomActive && enteredroom != this)
        {
            DeactivateChildren();
            roomActive = false;
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnterRoom(this);
        roomActive = true;
        ActivateChildren();
    }

    public Collider2D GetCollider2D()
    {
        return col2D;
    }

    void DeactivateChildren()
    {
        foreach (GameObject child in childObjects)
        {
            child.SetActive(false);
        }
    }
    
    void ActivateChildren()
    {
        foreach (GameObject child in childObjects)
        {
            child.SetActive(true);
        }
    }

    void GetRoomChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (!child.name.Contains(name))
            {
                childObjects.Add(child);
            }
        }
    }
}
