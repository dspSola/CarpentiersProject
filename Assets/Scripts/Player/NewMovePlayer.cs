using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovePlayer : MonoBehaviour
{
    [SerializeField] private HorizontalStateMachine _horizontalStateMachine;
    [SerializeField] private VerticalStateMachine _verticalStateMachine;
    [SerializeField] private NewRayCastFloorManager _rayCastFloorManager;
    [SerializeField] private Transform _parentPlayerTr;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float _inputHorizontal;
    [SerializeField] private Vector2 _velocity;

    [SerializeField] private bool _pressJump, _isInJump;
    [SerializeField] private float _speedX, _forceJump, _timeInJump, _timeInJumpMax, _timeToFall, _maxVelocityY;
    [SerializeField] [Range(0f, 2f)] private float _coefGravity;
    [SerializeField] private Vector2 _rigidbodyOnFloorPosition;
    [SerializeField] private float _floorOffsetY;

    private void Update()
    {
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _pressJump = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        MoveX();

        if (_verticalStateMachine.CurrentState == PlayerVerticalState.FALLING)
        {
            Falling();
        }
        if (_verticalStateMachine.CurrentState == PlayerVerticalState.JUMPING)
        {
            Jump();
        }
        if (_verticalStateMachine.CurrentState == PlayerVerticalState.GROUNDED)
        {
            StickToGround();
        }

        _rigidBody2D.velocity = _velocity;
    }

    // Horizontal
    public void MoveX()
    {
        if (_horizontalStateMachine.CurrentState != PlayerHorizontalState.IDLE)
        {
            _velocity.x = _inputHorizontal * _speedX;
        }
    }

    public void ExitMoveX()
    {
        _velocity.x = 0;
    }

    // Vertical
    // Fall
    public void EnterFalling()
    {
        _timeToFall = 0;
    }
    public void Falling()
    {
        _timeToFall += Time.deltaTime;
        _velocity.y = Physics2D.gravity.y * _timeToFall * _coefGravity;
        Mathf.Clamp(_velocity.y, -_maxVelocityY, _maxVelocityY);
    }
    public void ExitFalling()
    {
        _velocity.y = 0;
    }

    // Ground
    public void EnterGrounded()
    {
        _velocity.y = 0;
    }

    public void ExitGrounded()
    {

    }

    private void StickToGround()
    {
        // Calcule la position y du sol
        _rayCastFloorManager.AverageCollision(out Vector2 floorPosition);

        // Calcule la position où doit se trouver le rigidbody en fonction du sol
        _rigidbodyOnFloorPosition = new Vector2(_rigidBody2D.position.x, floorPosition.y + _floorOffsetY);

        // Si la position a changé
        //if (_rigidBody2D.position.y < _rigidbodyOnFloorPosition.y + 0.05f && _rigidBody2D.position.y > _rigidbodyOnFloorPosition.y - 0.05f)
        //{
        //    Debug.Log("Tp player");
        //    //_parentPlayerTr.position = new Vector3(_parentPlayerTr.position.x, _rigidbodyOnFloorPosition.y);
        //    _rigidBody2D.MovePosition(_rigidbodyOnFloorPosition);
        //}

        //if (!_rigidbodyOnFloorPosition.Approximately(_rigidBody2D.position))
        //{
        //    Debug.Log("Tp player");

        //    //Téléporte le rigidbody là où il doit être par rapport au sol
        //    _rigidBody2D.MovePosition(_rigidbodyOnFloorPosition);

        //    //Annule la velocity verticale
        //    _velocity.y = 0;
        //}
    }

    // Jump
    public void EnterJump()
    {
        _timeToFall = 0;
        _isInJump = true;
    }

    public void Jump()
    {
        if (_pressJump)
        {
            if (_timeInJump < _timeInJumpMax)
            {
                _timeInJump += Time.deltaTime;
                //_rigidBody2D.AddForce(new Vector2(0, _forceJump - (_timeInJump / 100)), ForceMode2D.Force);
                _rigidBody2D.AddForce(new Vector2(0, _forceJump), ForceMode2D.Force);
            }
            else
            {
                ExitJump();
            }
        }
        else
        {
            ExitJump();
        }
    }

    public void ExitJump()
    {
        _timeInJump = 0;
        _isInJump = false;
    }

    public float InputHorizontal { get => _inputHorizontal; set => _inputHorizontal = value; }
    public bool PressJump { get => _pressJump; set => _pressJump = value; }
    public bool IsInJump { get => _isInJump; set => _isInJump = value; }
}
