using UnityEngine;

public class AnimatorData
{
    public static class Params
    {
        public static readonly int HashRun = Animator.StringToHash("Run");
        public static readonly int HashCarry = Animator.StringToHash("Carry");
        public static readonly int HashIdle = Animator.StringToHash("Idle");
    }
}
