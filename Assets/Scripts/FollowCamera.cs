using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
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
        if (player.GetVelocity().x > 3f &&
            player.IsFacingRight())
        {
            vcamFrame.m_ScreenX = 0.25f;
        }
        else if (player.GetVelocity().x < -3f &&
            !player.IsFacingRight())
        {
            vcamFrame.m_ScreenX = 0.75f;

        }
    }

    void ChangeRoom(Room room)
    {
        confiner.InvalidatePathCache();
        confiner.m_BoundingShape2D = room.GetCollider2D();
    }
}
