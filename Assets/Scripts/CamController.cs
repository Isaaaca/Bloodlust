using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController: MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera followCam = null;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera centeredCam = null;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera jumpCam = null;

    public enum CameraMode
    {
        Follow,
        Center,
        Jump
    }

    private CameraMode camMode = CameraMode.Follow;

    public void SwitchCamera(CameraMode mode)
    {
        if(camMode != mode)
        {
            if (mode == CameraMode.Follow)
            {
                followCam.enabled = true;
                centeredCam.enabled = false;
                jumpCam.enabled = false;
                camMode = CameraMode.Follow;
            }
            else if (mode == CameraMode.Center)
            {
                followCam.enabled = false;
                centeredCam.enabled = true;
                jumpCam.enabled = false;
                camMode = CameraMode.Center;
            }
            else if (mode == CameraMode.Jump)
            {
                followCam.enabled = false;
                centeredCam.enabled = false;
                jumpCam.enabled = true;
                camMode = CameraMode.Jump;
            }
        }
    }

    public void JumpToTarget(Transform target)
    {
        SwitchCamera(CameraMode.Jump);
        ChangeFollowTarget(target);
    }
    public void PanToTarget(Transform target)
    {
        SwitchCamera(CameraMode.Center);
        ChangeFollowTarget(target);
    }

    public void ChangeFollowTarget(Transform target)
    {
        GetCurrentVcam().Follow = (target);
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
        else if (camMode == CameraMode.Jump)
            return jumpCam;
        else
            return null;
    }
}
