using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerController controller;
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
}
