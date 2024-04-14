using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingRat : MonoBehaviour
{

    [SerializeField] private int _offset;

    private Queue<Vector3> _positionQueue;

    private Vector2 _nextPos;

    private void Awake()
    {
        _positionQueue = new Queue<Vector3>();
    }
    private void FixedUpdate()
    {
        _positionQueue.Enqueue(Player.Inst.controller.transform.position);
        if(_positionQueue.Count>_offset)
        {
            transform.position = _positionQueue.Dequeue();
        }

    }
    public void Clear()
    {
        _positionQueue = new Queue<Vector3>();
    }
}
