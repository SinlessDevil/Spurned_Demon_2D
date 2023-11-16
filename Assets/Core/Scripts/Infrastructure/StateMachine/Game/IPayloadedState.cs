namespace Infrastructure.StateMachine.Game
{
    public interface IPayloadedState<TPayload> : IExitable
    {
        void Enter(TPayload payload);
    }
}