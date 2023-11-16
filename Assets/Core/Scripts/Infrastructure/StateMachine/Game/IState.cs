namespace Infrastructure.StateMachine.Game
{
    public interface IState : IExitable
    {
        void Enter();
    }
}