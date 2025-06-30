using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState { get; private set; } // 对外可读不可写

    public void Initialize(PlayerState _startState)
    {
        // 初始化状态
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        // 改变状态
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
