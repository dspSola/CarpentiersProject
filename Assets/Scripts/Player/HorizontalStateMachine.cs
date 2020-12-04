using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerHorizontalState
{
    LEFT,
    IDLE,
    RIGHT,
}

public class HorizontalStateMachine : MonoBehaviour
{
    public GUIStyle myStyle;

    [SerializeField] private NewMovePlayer _movePlayer;
    [SerializeField] private bool _debugOnGui;
    public PlayerHorizontalState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    private void Start()
    {
        TransitionToState(_currentState, PlayerHorizontalState.IDLE);
    }

    private void Update()
    {
        DoUpdate();
    }

    public void DoUpdate()
    {
        OnStateUpdate(_currentState);
    }

    private void OnStateEnter(PlayerHorizontalState state)
    {
        switch (state)
        {
            case PlayerHorizontalState.LEFT:
                DoLeftEnter();
                break;

            case PlayerHorizontalState.IDLE:
                DoIdleEnter();
                break;

            case PlayerHorizontalState.RIGHT:
                DoRightEnter();
                break;

            default:
                Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
                break;
        }
    }

    private void OnStateExit(PlayerHorizontalState state)
    {
        switch (state)
        {
            case PlayerHorizontalState.LEFT:
                DoLeftExit();
                break;

            case PlayerHorizontalState.IDLE:
                DoIdleExit();
                break;

            case PlayerHorizontalState.RIGHT:
                DoRightExit();
                break;

            default:
                Debug.LogError("OnStateExit: Invalid state " + state.ToString());
                break;
        }
    }

    private void OnStateUpdate(PlayerHorizontalState state)
    {
        switch (state)
        {
            case PlayerHorizontalState.LEFT:
                DoLeftUpdate();
                break;

            case PlayerHorizontalState.IDLE:
                DoIdleUpdate();
                break;

            case PlayerHorizontalState.RIGHT:
                DoRightUpdate();
                break;

            default:
                Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
                break;
        }
    }

    private void TransitionToState(PlayerHorizontalState fromState, PlayerHorizontalState toState)
    {
        OnStateExit(fromState);
        _currentState = toState;
        OnStateEnter(toState);
    }

    private void TransitionToState(PlayerHorizontalState toState)
    {
        TransitionToState(_currentState, toState);
    }

    // Left
    private void DoLeftEnter()
    {

    }
    private void DoLeftUpdate()
    {
        if (_movePlayer.InputHorizontal == 0)
        {
            TransitionToState(PlayerHorizontalState.IDLE);
            return;
        }
        if (_movePlayer.InputHorizontal > 0)
        {
            TransitionToState(PlayerHorizontalState.RIGHT);
            return;
        }
    }
    private void DoLeftExit()
    {
        _movePlayer.ExitMoveX();
    }

    // Idle
    private void DoIdleEnter()
    {

    }
    private void DoIdleUpdate()
    {
        if (_movePlayer.InputHorizontal < 0)
        {
            TransitionToState(PlayerHorizontalState.LEFT);
            return;
        }

        if (_movePlayer.InputHorizontal > 0)
        {
            TransitionToState(PlayerHorizontalState.RIGHT);
            return;
        }
    }
    private void DoIdleExit()
    {

    }

    // Right
    private void DoRightEnter()
    {

    }
    private void DoRightUpdate()
    {
        if (_movePlayer.InputHorizontal == 0)
        {
            TransitionToState(PlayerHorizontalState.IDLE);
            return;
        }
        if (_movePlayer.InputHorizontal < 0)
        {
            TransitionToState(PlayerHorizontalState.LEFT);
            return;
        }
    }
    private void DoRightExit()
    {
        _movePlayer.ExitMoveX();
    }

    private PlayerHorizontalState _currentState;

    private void OnGUI()
    {
        if (_debugOnGui)
        {
            GUI.Box(new Rect(0, 0 + Screen.height * 0.8f, Screen.width, Screen.height * 0.25f), _currentState.ToString(), myStyle);
        }
    }
}
