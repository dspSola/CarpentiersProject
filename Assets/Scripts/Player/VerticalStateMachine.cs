using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerVerticalState
{
    GROUNDED,
    JUMPING,
    FALLING,
}

public class VerticalStateMachine : MonoBehaviour
{
    public PlayerVerticalState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    private void Start()
    {
        TransitionToState(_currentState, PlayerVerticalState.FALLING);
    }

    private void Update()
    {
        DoUpdate();
    }

    public void DoUpdate()
    {
        OnStateUpdate(_currentState);
    }

    private void OnStateEnter(PlayerVerticalState state)
    {
        switch (state)
        {
            case PlayerVerticalState.GROUNDED:
                DoGroundedEnter();
                break;

            case PlayerVerticalState.JUMPING:
                DoJumpingEnter();
                break;

            case PlayerVerticalState.FALLING:
                DoFallingEnter();
                break;

            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }

    private void OnStateExit(PlayerVerticalState state)
    {
        switch (state)
        {
            case PlayerVerticalState.GROUNDED:
                DoGroundedExit();
                break;

            case PlayerVerticalState.JUMPING:
                DoJumpingExit();
                break;

            case PlayerVerticalState.FALLING:
                DoFallingExit();
                break;

            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }

    private void OnStateUpdate(PlayerVerticalState state)
    {
        switch (state)
        {
            case PlayerVerticalState.GROUNDED:
                DoGroundedUpdate();
                break;

            case PlayerVerticalState.JUMPING:
                DoJumpingUpdate();
                break;

            case PlayerVerticalState.FALLING:
                DoFallingUpdate();
                break;

            default:
                Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
                break;
        }
    }

    private void TransitionToState(PlayerVerticalState fromState, PlayerVerticalState toState)
    {
        OnStateExit(fromState);
        _currentState = toState;
        OnStateEnter(toState);
    }

    private void TransitionToState(PlayerVerticalState toState)
    {
        TransitionToState(_currentState, toState);
    }

#endregion


    #region State Grounded

    private void DoGroundedEnter()
    {
        _bruteAnimatorController.SetGrounded(true);
        _timeToJump = 0;
    }

    private void DoGroundedExit()
    {
        _bruteAnimatorController.SetGrounded(false);
    }

    private void DoGroundedUpdate()
    {
        if (!_groundCheck.TestCollision())
        {
            TransitionToState(PlayerVerticalState.FALLING);
            return;
        }

        if (_timeToJump < _timeToJumpMax)
        {
            _timeToJump += Time.deltaTime;
        }
        else if (_getBruteInput.JumpInput.IsActive && !_stateMachineAttack.IsAnim && !_getBruteInput.Attack01Input.IsActive)
        {
            TransitionToState(PlayerVerticalState.JUMPING);
            return;
        }
    }

    #endregion


    #region State Jumping

    private void DoJumpingEnter()
    {
        _playerMove.DoJump();
        _bruteAnimatorController.SetJumping(true);
    }

    private void DoJumpingExit()
    {
        _bruteAnimatorController.SetJumping(false);
    }

    private void DoJumpingUpdate()
    {
        if (_playerMove.VelocityRb.y < -0.0000001f)
        {
            TransitionToState(PlayerVerticalState.FALLING);
            return;
        }
    }

    #endregion


    #region State Falling

    private void DoFallingEnter()
    {
        _bruteAnimatorController.SetFalling(true);
    }

    private void DoFallingExit()
    {
        _bruteAnimatorController.SetFalling(false);
    }

    private void DoFallingUpdate()
    {
        if (_groundCheck.TestCollision())
        {
            TransitionToState(PlayerVerticalState.GROUNDED);
            return;
        }
    }

    #endregion

    private PlayerVerticalState _currentState;
}
