using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    [SerializeField] GameObject[] activatablesObjects;
    private IActivatable[] activatables;

    private void Awake()
    {
        //activatables = GetComponentsInChildren<IActivatable>();
        activatables = new IActivatable[activatablesObjects.Length];
        for(int i=0;i<activatablesObjects.Length;i++)
        {
            activatables[i] = activatablesObjects[i].GetComponent<IActivatable>();
        }
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
