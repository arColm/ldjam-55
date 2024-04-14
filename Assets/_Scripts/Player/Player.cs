using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour
{
    public static event Action Death;
    public Vector2 RespawnPoint;

    public PlayerController controller;
    public PlayerFlute flute;
    public static Player Inst;

    public bool PackRatsFound = false;
    public bool ResetRatsFound = false;
    public bool BulletRatsFound = false;
    public bool AntiGravityRatFound = false;

    [SerializeField] private ParticleSystem _deathParticles;
    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Inst = this;
        }
        PlayerController.EnterAntiGravity += FlipDeathParticles;

    }

    private void OnDestroy()
    {
        PlayerController.EnterAntiGravity -= FlipDeathParticles;
    }

    private void FlipDeathParticles(bool inAntiGravity)
    {
        var force = _deathParticles.forceOverLifetime;
        force.y = new MinMaxCurve(inAntiGravity? 5:-5);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Die();
        }
    }

    public void Die()
    {
        EmitDeathParticles();
        controller.ResetVelocity();
        controller.transform.position = RespawnPoint;
        flute.ResetRats();
        Death?.Invoke();
    }
    public void EmitDeathParticles()
    {
        EmitParams emitParams = new EmitParams();
        emitParams.position = controller.transform.position;

        _deathParticles.Emit(emitParams,15);
    }
    public void EmitDeathParticles(Vector2 position,int count)
    {
        EmitParams emitParams = new EmitParams();
        emitParams.position = position;

        _deathParticles.Emit(emitParams, count);
    }
}
