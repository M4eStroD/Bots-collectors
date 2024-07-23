using System;
using System.Collections.Generic;

public class StateMachine
{
    private readonly Dictionary<Type, IExitableState> _stateMap;

    public IExitableState CurrentState { get; private set; }

    public StateMachine(Dictionary<Type, IExitableState> stateMap)
    {
        _stateMap = stateMap;
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }

    public void Enter<T>() where T : class, IState
    {
        T state = ChangeState<T>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);
    }

    private T ChangeState<T>() where T : class, IExitableState
    {
        CurrentState?.Exit();
        T state = GetState<T>();    
        CurrentState = state;

        return state;
    }

    private T GetState<T>() where T : class, IExitableState
    {
        Type type = typeof(T);

        return _stateMap[type] as T;
    }
}