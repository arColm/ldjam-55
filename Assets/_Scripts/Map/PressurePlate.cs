using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    IActivatable[] activatables;

    private void Awake()
    {
        activatables = GetComponentsInChildren<IActivatable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Player.Inst.tag) || collision.CompareTag("PackRat"))
        {
            foreach(IActivatable activatable in activatables)
            {
                activatable.Activate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.Inst.tag) || collision.CompareTag("PackRat"))
        {
            foreach (IActivatable activatable in activatables)
            {
                activatable.Deactivate();
            }
        }
    }
}
