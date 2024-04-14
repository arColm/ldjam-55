using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{

    private Vector2 _startPos;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector2(_startPos.x,_startPos.y + Mathf.Sin(Time.time)/2);
    }
}
