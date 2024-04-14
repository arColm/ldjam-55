using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 RespawnPoint;

    public PlayerController controller;
    public PlayerFlute flute;
    public static Player Inst;

    public bool PackRatsFound = false;
    public bool ResetRatsFound = false;
    public bool BulletRatsFound = false;
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
        controller.ResetVelocity();
        controller.transform.position = RespawnPoint;
        flute.ResetRats();
    }
}
