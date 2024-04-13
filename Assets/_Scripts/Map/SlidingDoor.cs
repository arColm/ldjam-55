using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour,IActivatable
{

    private Vector2 _closedPosition;
    [SerializeField] private Vector2 _openPosition;

    private Coroutine _currentCoroutine;

    public void Activate()
    {
        print("OPEN");
        if(_currentCoroutine!=null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(DoOpen());
    }

    IEnumerator DoOpen()
    {
        while(transform.position.x != _openPosition.x || transform.position.y != _openPosition.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, _openPosition, Time.deltaTime * 5);
            yield return null;
        }
    }

    IEnumerator DoClose()
    {
        while (transform.position.x != _closedPosition.x || transform.position.y != _closedPosition.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, _closedPosition, Time.deltaTime * 5);
            yield return null;
        }

    }

    public void Deactivate()
    {
        print("CLOSE");
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(DoClose());
    }

    private void Awake()
    {
        _closedPosition = transform.position;
    }
}
