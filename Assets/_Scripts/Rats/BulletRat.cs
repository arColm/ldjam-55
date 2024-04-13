using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRat : Rat
{
    [SerializeField] private Vector2 _velocity;

    private bool _isFacingRight;


    private bool _isStuck = false;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private Controller2D _controller;
    private void Awake()
    {
        _controller = GetComponent<Controller2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Instantiate(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
        _spriteRenderer.flipX = isFacingRight;
        if (_isFacingRight) _velocity = new Vector2(20, 0);
        else _velocity = new Vector2(-20, 0);
    }

    private void FixedUpdate()
    {
        if(GameManager.Inst.state==GameManager.State.Playing)
        {
            if (!_isStuck)
            {
                if (_controller.collisions.left || _controller.collisions.right)
                {
                    _isStuck = true;
                    _rigidBody.simulated = true;
                }

                _controller.Move(_velocity * Time.fixedDeltaTime);
            }
        }

    }


}
