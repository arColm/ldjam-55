using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{

    public static event Action DoLevelSwitch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Player.Inst.tag))
        {
            CameraController.Inst.SetCurrentCamera(GetComponent<CinemachineVirtualCamera>());

            DoLevelSwitch?.Invoke();
        }
    }
}
