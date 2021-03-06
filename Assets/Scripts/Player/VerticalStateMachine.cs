﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerVerticalState
{
    GROUNDED,
    JUMPING,
    CLIMBING,
    FALLING,
}

public class VerticalStateMachine : MonoBehaviour
{
    public GUIStyle myStyle;
    [SerializeField] private NewMovePlayer _movePlayer;
    [SerializeField] private PlayerAnimatorController _playerAnimatorController;
    [SerializeField] private RayCastDetection _rayCastDetectionUp, _rayCastDetectionRight, _rayCastDetectionDown, _rayCastDetectionLeft;
    [SerializeField] private OverLapFloor _overLapFloor;
    [SerializeField] private bool _debugOnGui;
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
        //myStyle.fontSize = (int)(8.0f * (float)(Screen.width) / 96.0f);
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
            case PlayerVerticalState.CLIMBING:
                DoClimbingEnter();
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
            case PlayerVerticalState.CLIMBING:
                DoClimbingExit();
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

            case PlayerVerticalState.CLIMBING:
                DoClimbingUpdate();
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

    // Ground
    private void DoGroundedEnter()
    {
        Debug.Log("DoGroundedEnter");
        _playerAnimatorController.SetGrounded(true);
        _movePlayer.EnterGrounded();
    }
    private void DoGroundedUpdate()
    {
        //if (!_overLapFloor.TestCollision())
        //{
        //    TransitionToState(PlayerVerticalState.FALLING);
        //    return;
        //}
        ToJumping();
        ToFalling();
    }
    private void DoGroundedExit()
    {
        _playerAnimatorController.SetGrounded(false);
        _movePlayer.ExitGrounded();
    }

    // Jump
    private void DoJumpingEnter()
    {
        _playerAnimatorController.SetJumping(true);
        _movePlayer.EnterJump();
    }
    private void DoJumpingUpdate()
    {
        //ToClimbing();
        if (!_movePlayer.IsInJump)
        {
            ToFalling();
        }
    }
    private void DoJumpingExit()
    {
        _playerAnimatorController.SetJumping(false);
    }

    // Climb
    private void DoClimbingEnter()
    {
        _playerAnimatorController.SetClimbing(true);
        _movePlayer.EnterClimb();
    }
    private void DoClimbingUpdate()
    {
        ToGrounded();
        ToJumping();
        ClimbingToFalling();
    }
    private void DoClimbingExit()
    {
        _playerAnimatorController.SetClimbing(false);
        _movePlayer.ExitClimb();
    }

    // Fall
    private void DoFallingEnter()
    {
        Debug.Log("DoFallingEnter");
        _playerAnimatorController.SetFalling(true);
        _movePlayer.EnterFalling();
    }
    private void DoFallingUpdate()
    {
        Debug.Log("DoFallingUpdate");

        //if(_overLapFloor.TestCollision())
        //{
        //    TransitionToState(PlayerVerticalState.GROUNDED);
        //    return;
        //}
        ToClimbing();
        ToGrounded();
    }
    private void DoFallingExit()
    {
        _playerAnimatorController.SetFalling(false);
        _movePlayer.ExitFalling();
    }

    private PlayerVerticalState _currentState;

    private void OnGUI()
    {    
        if (_debugOnGui)
        {
            GUI.Box(new Rect(0, 0 - Screen.height * 0.0625f, Screen.width, Screen.height * 0.25f), _currentState.ToString(), myStyle);
        }
    }

    private void ToGrounded()
    {
        if (_rayCastDetectionDown.IfOnOfRayCastTouch)
        {
            //_movePlayer.RalentitChute();
            if (_rayCastDetectionDown.MinDistanceHit < 0.025f)
            {
                TransitionToState(PlayerVerticalState.GROUNDED);
                return;
            }
        }
    }
    private void ToJumping()
    {
        if (_movePlayer.PressJump)
        {
            TransitionToState(PlayerVerticalState.JUMPING);
            return;
        }
    }
    private void ToClimbing()
    {
        if(_rayCastDetectionLeft.IfOnOfRayCastTouch || _rayCastDetectionRight.IfOnOfRayCastTouch)
        {
            if (_rayCastDetectionLeft.MinDistanceHit <= 0.02f || _rayCastDetectionRight.MinDistanceHit <= 0.02f)
            {
                TransitionToState(PlayerVerticalState.CLIMBING);
                return;
            }
        }
    }
    private void ToFalling()
    {
        if (_rayCastDetectionDown.MinDistanceHit > 0.025f)
        {
            TransitionToState(PlayerVerticalState.FALLING);
            return;
        }
    }

    private void ClimbingToFalling()
    {
        if (_rayCastDetectionLeft.MinDistanceHit > 0.02f && _rayCastDetectionRight.MinDistanceHit > 0.02f)
        {
            TransitionToState(PlayerVerticalState.FALLING);
            return;
        }
    }

}
