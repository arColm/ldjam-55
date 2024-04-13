using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackRats : Rat
{
    private Vector2 _velocity;
    private float _terminalVelocity = -30;
    private float _gravity = -40;


    private Controller2D _controller;
    private void Awake()
    {
        _controller = GetComponent<Controller2D>();
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
                //gravity
                if (_velocity.y >= _terminalVelocity)
                    _velocity.y += _gravity * Time.fixedDeltaTime;



                _controller.Move(_velocity * Time.fixedDeltaTime);
            }




        }
    }
}
