using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{

    public static event Action DoLevelSwitch;

    [SerializeField] private Vector2 _respawnPoint;

    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        Player.Death += Shake;
    }
    private void OnDestroy()
    {
        Player.Death -= Shake;
    }



    private void Shake()
    {
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        CinemachineBasicMultiChannelPerlin noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 2;
        yield return new WaitForSecondsRealtime(0.2f);
        noise.m_AmplitudeGain = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Player.Inst.tag))
        {
            CameraController.Inst.SetCurrentCamera(_camera);

            Player.Inst.RespawnPoint = _respawnPoint;

            DoLevelSwitch?.Invoke();
        }
    }
}
