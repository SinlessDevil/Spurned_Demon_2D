using Services.StaticData;
using Zenject;

namespace Services.Factories.Game
{
    public class GameFactory : Factory, IGameFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;

        public GameFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }
    }
}