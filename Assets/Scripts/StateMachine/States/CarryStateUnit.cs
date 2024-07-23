using System;
using UnityEngine;

public class CarryStateUnit : IPayloadState<Vector3>
{
    private readonly Animator _animator;
    private readonly StateMachine _stateMachine;
    private readonly UnitMover _unitMover;

    private Vector3 _position;

    public event Action ReachedDestination;

    public CarryStateUnit(Animator animator, StateMachine stateMachine, UnitMover unitMover)
    {
        _animator = animator;
        _stateMachine = stateMachine;
        _unitMover = unitMover;
    }

    public void Enter(Vector3 position)
    {
        _position = position;

        _animator.SetTrigger(AnimatorData.Params.HashCarry);
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
        if (_unitMover.TryMoveTowards(_position, Constants.DistanceOffset) == false)
            ReachedDestination?.Invoke();
    }

    public void Update()
    {
    }
}
