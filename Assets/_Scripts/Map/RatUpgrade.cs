using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatUpgrade : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.Inst.tag))
        {
            Player.Inst.flute.UpgradeRats();
            Player.Inst.EmitDeathParticles();
            Destroy(gameObject);
        }
    }


}
