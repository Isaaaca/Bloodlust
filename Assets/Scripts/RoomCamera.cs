using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    private Cinemachine.CinemachineConfiner confiner;
    private Cinemachine.CinemachineVirtualCamera vcam;
    private Cinemachine.CinemachineFramingTransposer vcamFrame;
    private Transform followTarget;
    [SerializeField]private PlayerController player = null;


    private void Awake()
    {
        Room.OnEnterRoom += ChangeRoom;
    }
    // Start is called before the first frame update
    void Start()
    {
        confiner = GetComponent<Cinemachine.CinemachineConfiner>();
        vcam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        vcamFrame = vcam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();
        followTarget = player.transform;
    }

    private void Update()
    {
        if (followTarget.position.x - vcamFrame.TrackedPoint.x < -1f)
        {
            vcamFrame.m_ScreenX = 0.35f;
        }
        else if (followTarget.position.x - vcamFrame.TrackedPoint.x > 1f)
        {
            vcamFrame.m_ScreenX = 0.65f;

        }
    }

    void ChangeRoom(Room room)
    {
        confiner.InvalidatePathCache();
        confiner.m_BoundingShape2D = room.GetCollider2D();
    }
}
