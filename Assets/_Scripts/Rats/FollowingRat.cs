using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingRat : MonoBehaviour
{

    [SerializeField] private int _offset;

    private Queue<Vector3> _positionQueue;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _nextPos;

    private void Awake()
    {
        _positionQueue = new Queue<Vector3>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        _positionQueue.Enqueue(Player.Inst.controller.transform.position);
        if(_positionQueue.Count>_offset)
        {
            Vector2 nextPos = _positionQueue.Dequeue();
            if (nextPos.x < transform.position.x) _spriteRenderer.flipX = false; //facing left
            else _spriteRenderer.flipX = true;
            transform.position = nextPos;
            
        }

    }
    public void Clear()
    {
        _positionQueue = new Queue<Vector3>();
    }
}
