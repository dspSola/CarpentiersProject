using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovePlayer : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private HorizontalStateMachine _horizontalStateMachine;
    [SerializeField] private VerticalStateMachine _verticalStateMachine;
    [SerializeField] private NewRayCastFloorManager _rayCastFloorManager;
    [SerializeField] private RayCastDetection _rayCastDetectionDown, _rayCastDetectionLeft, _rayCastDetectionRight;
    [SerializeField] private PlayerAnimatorController _playerAnimatorController;
    [SerializeField] private Transform _parentPlayerTr;
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float _inputHorizontal, _speedInputHorizontal, _accelerationSpeedInputHorizontal;
    [SerializeField] private Vector2 _velocity;

    [SerializeField] private bool _pressJump, _isInJump;
    [SerializeField] private float _speed, _speedWalk, _speedMini, _forceJump, _maxVelocityY, _timeInJump, _timeInJumpMax, _timeToFall, _timeToClimb, _climbForcePush, _cptGameFlowTransform;
    [SerializeField] [Range(0f, 2f)] private float _coefGravity;
    [SerializeField] private Vector2 _rigidbodyOnFloorPosition;
    [SerializeField] private float _floorOffsetY;

    [SerializeField] private Transform _transformHitDown, _lastTransformHitDown, _transformHitClimb, _lastTransformHitClimb;

    private void Awake()
    {
        _speed = _speedMini;
    }

    private void Start()
    {

    }

    private void Update()
    {
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _playerAnimatorController.SetinputX(_inputHorizontal);
        _pressJump = Input.GetButton("Jump");
        _playerData.PositionPlayer = _parentPlayerTr.position;

        if (_horizontalStateMachine.CurrentState != PlayerHorizontalState.IDLE)
        {
            if (_speed < _speedWalk)
            {
                _speed += Time.deltaTime * _accelerationSpeedInputHorizontal;
            }
        }
        else
        {
            if (_speed > _speedMini)
            {
                _speed -= Time.deltaTime * _accelerationSpeedInputHorizontal * 2;
            }
        }
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
        if (_verticalStateMachine.CurrentState == PlayerVerticalState.CLIMBING)
        {
            Climb();
        }
        if (_verticalStateMachine.CurrentState == PlayerVerticalState.GROUNDED)
        {
            //StickToGround();
        }

        _rigidBody2D.velocity = _velocity;
    }

    // Horizontal
    public void EnterLeftOrRight()
    {

    }
    public void MoveX()
    {
        _velocity.x = _inputHorizontal * _speed * Time.fixedDeltaTime;
    }
    public void ExitLeftOrRight()
    {

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

        if(_rayCastDetectionDown.MinDistanceHit < 0.02f)
        {
            RalentitChute();        
        }
        else
        {
            _velocity.y = Physics2D.gravity.y * _timeToFall * _coefGravity;
        }
        Mathf.Clamp(_velocity.y, -_maxVelocityY, _maxVelocityY);
    }
    public void ExitFalling()
    {
        _velocity.y = 0;
    }

    // Ground
    public void EnterGrounded()
    {
        _transformHitDown = _rayCastDetectionDown.TransformHit;
        SetParent(_transformHitDown);
        
        if(_transformHitDown != _lastTransformHitDown)
        {
            _cptGameFlowTransform++;
            _playerData.GameFlow += 0.1f * _cptGameFlowTransform;
        }
        else
        {
            _playerData.GameFlow -= 0.1f * _cptGameFlowTransform / 2;
            _cptGameFlowTransform = 1;
        }
        _lastTransformHitDown = _transformHitDown;
    }
    public void ExitGrounded()
    {
        SetNoneParent();
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

    // Climb
    public void EnterClimb()
    {
        _timeToClimb = 0;
        _climbForcePush = 0;
        if(_horizontalStateMachine.CurrentState == PlayerHorizontalState.LEFT)
        {
            _transformHitClimb = _rayCastDetectionLeft.TransformHit;
            SetParent(_transformHitClimb);
        }
        else if (_horizontalStateMachine.CurrentState == PlayerHorizontalState.RIGHT)
        {
            _transformHitClimb = _rayCastDetectionRight.TransformHit;
            SetParent(_rayCastDetectionRight.TransformHit);
        }

        if (_transformHitClimb != _lastTransformHitClimb)
        {
            _cptGameFlowTransform++;
            _playerData.GameFlow += 0.1f * _cptGameFlowTransform;
        }
        else
        {
            _playerData.GameFlow -= 0.1f * _cptGameFlowTransform;
            _cptGameFlowTransform = 1;
        }
        _lastTransformHitClimb = _transformHitClimb;
    }
    private void Climb()
    {
        _timeToClimb += Time.deltaTime;
        _velocity.y = (Physics2D.gravity.y / 2f) * _timeToClimb * _coefGravity / 2;
        Mathf.Clamp(_velocity.y, -_maxVelocityY / 2, _maxVelocityY / 2);

        _playerData.GameFlow -= _timeToClimb * 0.0001f;
    }
    public void ExitClimb()
    {
        if (_inputHorizontal < 0)
        {
            _climbForcePush = -0.01f;
        }
        else if (_inputHorizontal > 0)
        {
            _climbForcePush = -0.01f;
        }
        else
        {
            _climbForcePush = 0f;
        }

        SetNoneParent();
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

    public void RalentitChute()
    {
        _velocity.y = Physics2D.gravity.y * _timeToFall * _rayCastDetectionDown.MinDistanceHit * _coefGravity / 2;
    }

    public void SetParent(Transform newParent)
    {
        _parentPlayerTr.parent = newParent;
    }
    public void SetNoneParent()
    {
        _parentPlayerTr.parent = null;
    }

    public float InputHorizontal { get => _inputHorizontal; set => _inputHorizontal = value; }
    public bool PressJump { get => _pressJump; set => _pressJump = value; }
    public bool IsInJump { get => _isInJump; set => _isInJump = value; }
    public float ClimbForcePush { get => _climbForcePush; set => _climbForcePush = value; }
}
