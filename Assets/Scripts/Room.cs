using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static event Action<Room> OnEnterRoom = (room) => { };

    [SerializeField] private float topBorder = 1;
    private PolygonCollider2D col2D;
    private void Awake()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        Bounds bounds = colliders[0].bounds;
        foreach (Collider2D col in colliders)
        {
            bounds.Encapsulate(col.bounds);
        }
        bounds.center += Vector3.up * (topBorder / 2);
        bounds.center -= transform.position;
        bounds.size += Vector3.up * topBorder;
        bounds.size -= Vector3.right*0.5f;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnterRoom(this);
    }

    public Collider2D GetCollider2D()
    {
        return col2D;
    }
}
