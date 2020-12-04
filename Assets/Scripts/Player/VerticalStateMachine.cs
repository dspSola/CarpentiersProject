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
    public GUIStyle myStyle;

    [SerializeField] private NewMovePlayer _movePlayer;
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

    // Ground
    private void DoGroundedEnter()
    {
        Debug.Log("DoGroundedEnter");
        _movePlayer.EnterGrounded();
    }
    private void DoGroundedUpdate()
    {
        if(_movePlayer.PressJump)
        {
            TransitionToState(PlayerVerticalState.JUMPING);
            return;
        }

        if (!_overLapFloor.TestCollision())
        {
            TransitionToState(PlayerVerticalState.FALLING);
            return;
        }
    }
    private void DoGroundedExit()
    {

    }

    // Jump
    private void DoJumpingEnter()
    {
        _movePlayer.EnterJump();
    }
    private void DoJumpingUpdate()
    {
        if(!_movePlayer.IsInJump)
        {
            TransitionToState(PlayerVerticalState.FALLING);
            return;
        }
    }
    private void DoJumpingExit()
    {

    }

    // Fall
    private void DoFallingEnter()
    {
        Debug.Log("DoFallingEnter");
        _movePlayer.EnterFalling();
    }
    private void DoFallingUpdate()
    {
        Debug.Log("DoFallingUpdate");

        if(_overLapFloor.TestCollision())
        {
            TransitionToState(PlayerVerticalState.GROUNDED);
            return;
        }
    }
    private void DoFallingExit()
    {

    }

    private PlayerVerticalState _currentState;

    private void OnGUI()
    {    
        if (_debugOnGui)
        {
            GUI.Box(new Rect(0, 0 - Screen.height * 0.0625f, Screen.width, Screen.height * 0.25f), _currentState.ToString(), myStyle);
        }
    }
}
