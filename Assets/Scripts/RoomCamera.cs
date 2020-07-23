using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    private Cinemachine.CinemachineConfiner confiner;

    private void Awake()
    {
        Room.OnEnterRoom += ChangeRoom;
    }
    // Start is called before the first frame update
    void Start()
    {
        confiner = GetComponent<Cinemachine.CinemachineConfiner>();
    }

    void ChangeRoom(Room room)
    {
        confiner.InvalidatePathCache();
        confiner.m_BoundingShape2D = room.GetCollider2D();
    }
}
