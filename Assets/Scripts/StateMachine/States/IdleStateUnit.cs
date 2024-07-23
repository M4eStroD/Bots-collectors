using UnityEngine;

public class IdleStateUnit : IState
{
    private readonly Animator _animator;

    public IdleStateUnit(Animator animator)
    {
        _animator = animator;
    }

    public void Enter()
    {
        _animator.SetTrigger(AnimatorData.Params.HashIdle);
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
    }
}
