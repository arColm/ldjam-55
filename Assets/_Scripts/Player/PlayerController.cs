using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 6;
    [SerializeField] private float _decelerationRateAirborne = 50;
    [SerializeField] private float _decelerationRateGrounded = 150;

    [SerializeField] private float _accelerationTimeAirborne = 0.1f;
    [SerializeField] private float _accelerationTimeGrounded = 0.05f;

    [SerializeField] private float _terminalVelocity = -20;
    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 4;
    [SerializeField] private float _timeToJumpApex = .4f;
    [SerializeField] private float _coyoteJumpDelay = 0.2f;
    [SerializeField] private float _jumpPressedDelay = 0.2f;

    [SerializeField] private float _gravity = -30;

    private Vector2 _velocity;
    private float _jumpVelocity;
    private float _velocityXSmoothing;


    private int _remainingJumps = 1;
    private float _jumpPressedTimer = 0f;

    private bool _inAntiGravity = false;


    private Controller2D _controller;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _coyoteJump = true;
    private bool _isGrounded = false;
    public bool _isFacingRight { private set; get; } = true;
    private bool _canCancelJump = false;
    [SerializeField] public State state { private set; get; }

    private bool _jumpPressed = false;
    private bool _jumpCancelled = false;
    private void Awake()
    {
        _controller = GetComponent<Controller2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpVelocity = Mathf.Abs(_gravity) * _timeToJumpApex;
    }

    private void Update()
    {
        if(GameManager.Inst.state!=GameManager.State.Paused)
        {
            /*          HANDLE INPUTS
            * ============================================================== */
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpPressed = true;
                _jumpPressedTimer = _jumpPressedDelay;
            }
            if(Input.GetKeyUp(KeyCode.Space) && _canCancelJump)
            {
                _jumpCancelled = true;
            }



            /*          
            * ============================================================== */
            if (_jumpPressedTimer>0)
            {
                _jumpPressedTimer -= Time.deltaTime;
                if (_jumpPressedTimer <= 0) _jumpPressed = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Inst.state != GameManager.State.Paused)
        {
            //handle collisions
            if (_controller.collisions.below && _velocity.y < 0)
            {
                _velocity.y = 0;
            }

            if (_controller.collisions.above && _velocity.y > 0)
            {
                _velocity.y = 0;
            }

            if (_controller.collisions.right && _velocity.x > 0)
            {
                _velocity.x = 0;
            }

            if (_controller.collisions.left && _velocity.x < 0)
            {
                _velocity.x = 0;
            }

            //coyote jump timer
            if (!_controller.collisions.below && _remainingJumps == 1)
            {
                StartCoroutine(StartCoyoteJumpTimer());
                _remainingJumps--;
            }
            _isGrounded = _controller.collisions.below;
            if (_isGrounded)
            {
                _remainingJumps = 1;
            }
            Vector2 input;
            if (_inAntiGravity) input = new Vector2(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            else input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            float targetVelocityX = 0;
            switch (state)
            {
                case State.Idle:

                    if (input.x != 0) SetFacingRight(input.x > 0);
                    //run
                    targetVelocityX = input.x * _moveSpeed;

                    if (_jumpPressed && _remainingJumps>0)
                    {
                        Jump();
                        if(_coyoteJump)
                        {
                            _coyoteJump = false;
                        }
                        else
                        {
                            _remainingJumps--;
                        }
                        UpdateState(State.Jumping);

                    }
                    if (!_isGrounded && Mathf.Abs(_velocity.y) > 1f)
                    {
                        UpdateState(State.Falling);
                    }
                    if (Mathf.Abs(_velocity.x) > 0.2f)
                        UpdateState(State.Walking);

                    break;
                case State.Walking:
                    if (input.x != 0) SetFacingRight(input.x > 0);
                    targetVelocityX = input.x * _moveSpeed;
                    if (_jumpPressed && _remainingJumps > 0)
                    {
                        Jump();
                        if (_coyoteJump)
                        {
                            _coyoteJump = false;
                        }
                        else
                        {
                            _remainingJumps--;
                        }
                        UpdateState(State.Jumping);

                    }
                    if (!_isGrounded)
                    {
                        UpdateState(State.Falling);
                    }
                    //idle
                    if (Mathf.Abs(_velocity.x) < 0.2f)
                        UpdateState(State.Idle);
                    break;
                case State.Jumping:

                    if (input.x != 0) SetFacingRight(input.x > 0);
                    targetVelocityX = input.x * _moveSpeed;
                    //cancel jump
                    if(_jumpCancelled && _velocity.y > 0 && _canCancelJump)
                    {
                        _velocity.y = _jumpVelocity * 0.1f;
                    }
                    //fall
                    if (_velocity.y < 0)
                        UpdateState(State.Falling);
                    //grounded
                    if (_isGrounded)
                    {
                        UpdateState(State.Idle);
                    }
                    break;
                case State.Falling:
                    if (input.x != 0) SetFacingRight(input.x > 0);

                    targetVelocityX = input.x * _moveSpeed;
                    if (_jumpPressed && _coyoteJump)
                    {
                        _jumpPressed = false;
                        Jump();
                        break;
                    }
                    //idle
                    if (_isGrounded)
                    {
                        if (Mathf.Abs(_velocity.x) > 0.2f) UpdateState(State.Walking);
                        else UpdateState(State.Idle);
                        break;
                    }
                    //jumping
                    if (_velocity.y > 0)
                    {
                        UpdateState(State.Jumping);
                        break;

                    }
                    break;
                case State.PlayingFlute:
                    break;
            }

            /*          MOVE CHARACTER
             * ============================================================== */
            if (targetVelocityX != 0)
            {
                _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, 
                    Time.fixedDeltaTime * 60 * ((_controller.collisions.below) ? _accelerationTimeGrounded : _accelerationTimeAirborne));
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, targetVelocityX, Time.fixedDeltaTime * (_isGrounded ? _decelerationRateGrounded : _decelerationRateAirborne));

            }

/*            if (_inAntiGravity)
            {
                //gravity
                if (_velocity.y <= -_terminalVelocity)
                    _velocity.y += -_gravity * Time.fixedDeltaTime;
            }
            else*/
            
                //gravity
                if (_velocity.y >= _terminalVelocity)
                    _velocity.y += _gravity * Time.fixedDeltaTime;
            


            _controller.Move(_velocity * Time.fixedDeltaTime);

            _jumpCancelled = false;
        }
    }

    public void SetInAntiGravity(bool inAntiGravity)
    {
        if(_inAntiGravity!=inAntiGravity)
        {
            _velocity = new Vector2(-_velocity.x, -_velocity.y/2);
        }
        _inAntiGravity = inAntiGravity;
        if(inAntiGravity)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
    private void Jump()
    {

/*        if(_inAntiGravity)
        {
            if (_velocity.y > 0) _velocity.y = 0;
            _velocity.y -= _jumpVelocity;
        }
        else
        {*/
            if (_velocity.y < 0) _velocity.y = 0;
            _velocity.y += _jumpVelocity;

        

        //CreateJumpParticles();
        //AudioManager.inst.PlayPlayerJumpSound();
        StartCoroutine(StartJumpCancelTimer());

        _jumpPressed = false;
    }
    IEnumerator StartCoyoteJumpTimer()
    {
        _coyoteJump = true;
        yield return new WaitForSeconds(_coyoteJumpDelay);
        _coyoteJump = false;
    }

    IEnumerator StartJumpCancelTimer()
    {
        _canCancelJump = true;
        yield return new WaitForSeconds(_timeToJumpApex * 0.8f);
        _canCancelJump = false;
    }
    public void SetFacingRight(bool right)
    {
        if (_isFacingRight != right)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x) * (right ? -1 : 1),
                transform.localScale.y,
                transform.localScale.z);
        }
        _isFacingRight = right;



    }

    private void UpdateState(State s)
    {
        state = s;
        switch (s)
        {
            case State.Idle:
                _animator.Play("Player_idle");
                break;
            case State.Walking:
                _animator.Play("Player_walking");
                break;
            case State.Jumping:
                _animator.Play("Player_jumping");
                break;
            case State.Falling:
                _animator.Play("Player_falling");
                break;
            case State.PlayingFlute:
                break;
        }
    }

    public enum State
    {
        Idle,
        Walking,
        Jumping,
        Falling,
        PlayingFlute
    }
}
