using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackRats : Rat
{
    private Vector2 _velocity;
    private float _terminalVelocity = -30;
    private float _gravity = -40;

    private bool _inAntiGravity = false;


    private Controller2D _controller;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _controller = GetComponent<Controller2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        if (GameManager.Inst.state != GameManager.State.Paused)
        {
            //handle collisions
            if (_controller.collisions.below && _velocity.y>0)
            {
                _velocity.y = 0;
            }
            else
            {
                if (_inAntiGravity)
                {
                    //gravity
                    if (_velocity.y <= -_terminalVelocity)
                        _velocity.y += -_gravity * Time.fixedDeltaTime;
                }
                else
                {
                    //gravity
                    if (_velocity.y >= _terminalVelocity)
                        _velocity.y += _gravity * Time.fixedDeltaTime;
                }



                _controller.Move(_velocity * Time.fixedDeltaTime);
            }




        }
    }

    public void SetInAntiGravity(bool inAntiGravity)
    {
        if (_inAntiGravity != inAntiGravity)
        {
            _velocity = new Vector2(-_velocity.x, -_velocity.y);
        }
        _inAntiGravity = inAntiGravity;
        _spriteRenderer.flipY = inAntiGravity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("AntiGravityZone"))
        {
            SetInAntiGravity(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("AntiGravityZone"))
        {
            SetInAntiGravity(false);
        }
    }
}
