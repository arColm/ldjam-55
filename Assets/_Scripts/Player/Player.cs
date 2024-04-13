using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 RespawnPoint;

    public PlayerController controller;
    public PlayerFlute flute;
    public static Player Inst;
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

    public void Die()
    {
        controller.transform.position = RespawnPoint;
        flute.ResetRats();
    }
}
