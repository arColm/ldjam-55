using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravityZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Player.Inst.tag))
        {
            Player.Inst.controller.SetInAntiGravity(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.Inst.tag))
        {
            Player.Inst.controller.SetInAntiGravity(false);
        }
    }
}
