using Infrastructure.StateMachine.Game;

namespace Infrastructure.StateMachine
{
    public interface IStateFactory
    {
        T GetState<T>() where T : class, IExitable;
    }
}