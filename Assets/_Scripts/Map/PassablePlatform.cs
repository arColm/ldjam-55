using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassablePlatform : MonoBehaviour
{

    Collider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if(Player.Inst.controller.transform.position.y>=transform.position.y)
        {
            _collider.enabled = true;
        }
        else
        {
            _collider.enabled = false;
        }
    }
}
