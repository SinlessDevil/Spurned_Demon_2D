using Controllers;

namespace Controllers
{
    public class CompositeController : IGameController
    {
        private readonly IGameController[] _gameControllers;

        public CompositeController(params IGameController[] gameControllers)
        {
            _gameControllers = gameControllers;
        }

        public void Activate()
        {
            for (var i = 0; i < _gameControllers.Length; i++)
            {
                _gameControllers[i].Activate();
            }
        }

        public void Deactivate()
        {
            for (var i = 0; i < _gameControllers.Length; i++)
            {
                _gameControllers[i].Deactivate();
            }
        }
    }
}