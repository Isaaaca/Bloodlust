using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController: MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera followCam = null;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera centeredCam = null;

    public enum CameraMode
    {
        Follow,
        Center
    }

    private CameraMode camMode = CameraMode.Follow;

    public void SwitchCamera(CameraMode mode)
    {
        if (mode == CameraMode.Follow)
        {
            followCam.enabled = true;
            centeredCam.enabled = false;
            camMode = CameraMode.Follow;
        }
        else if (mode == CameraMode.Center)
        {
            followCam.enabled = false;
            centeredCam.enabled = true;
            camMode = CameraMode.Center;
        }
    }

    public void ChangeFollowTarget(Transform target)
    {
        centeredCam.Follow = target;
    }

    public CameraMode CurrentCameraMode()
    {
        return camMode;
    }

    private Cinemachine.CinemachineVirtualCamera GetCurrentVcam()
    {
        if (camMode == CameraMode.Follow)
            return followCam;
        else if (camMode == CameraMode.Center)
            return centeredCam;
        else
            return null;
    }
}
