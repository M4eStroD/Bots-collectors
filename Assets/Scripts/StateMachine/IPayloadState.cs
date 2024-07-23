public interface IPayloadState<T> : IExitableState
{
    void Enter(T payload);
}
