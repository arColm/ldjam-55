using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    [SerializeField] GameObject[] activatablesObjects;
    private IActivatable[] activatables;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite _activatedSprite;
    [SerializeField] private Sprite _deactivatedSprite;

    private int _weight=0;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
            _weight++;
            foreach(IActivatable activatable in activatables)
            {
                activatable.Activate();
            }
            _spriteRenderer.sprite = _activatedSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.Inst.tag) || collision.CompareTag("PackRat"))
        {
            _weight--;
            if(_weight==0)
            {
                foreach (IActivatable activatable in activatables)
                {
                    activatable.Deactivate();
                }
                _spriteRenderer.sprite = _deactivatedSprite;

            }

        }
    }
}
