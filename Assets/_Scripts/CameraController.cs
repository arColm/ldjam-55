using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Inst;
    [SerializeField] CinemachineVirtualCamera currentCamera;

    private void Awake()
    {
        currentCamera.Priority = 10;
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Inst = this;
        }
    }






    public void SetCurrentCamera(CinemachineVirtualCamera camera)
    {
        //GameManager.Inst.PauseGame(1);
        currentCamera.Priority = 0;
        currentCamera = camera;
        currentCamera.Priority = 10;
    }
}
