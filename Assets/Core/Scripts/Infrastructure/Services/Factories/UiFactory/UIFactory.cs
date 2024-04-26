using Infrastructure.Services.StaticData;
using Infrastructure.StaticData;
using UnityEngine;
using UI.Hudes;
using UI.Window;
using Zenject;

namespace Infrastructure.Services.Factories.UIFactory
{
    public class UIFactory : Factory, IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;

        private Transform _uiRoot;

        public GameHud GameHud { get; private set; }
        public MenuHud MenuHud { get; private set; }
        
        public UIFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }

        public RectTransform CrateWindow(WindowTypeId windowTypeId)
        {
            WindowConfig config = _staticData.ForWindow(windowTypeId);
            GameObject window = Instantiate(config.Prefab, _uiRoot);
            return window.GetComponent<RectTransform>();
        }

        public void CreateUiRoot() => _uiRoot = Instantiate(Path.UiRootPath).transform;
        public GameHud CreateGameHud()
        {
            GameHud = Instantiate(Path.GameHudPath).GetComponent<GameHud>();
            return GameHud;
        }
        public MenuHud CreateMenuHud()
        {
            MenuHud = Instantiate(Path.MenuHudPath).GetComponent<MenuHud>();
            return MenuHud;
        }
        
        private PathResourcesStaticData Path => _staticData.PathResourcesConfig;
    }
}