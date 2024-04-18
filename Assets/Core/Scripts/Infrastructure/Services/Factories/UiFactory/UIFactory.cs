using Infrastructure.Services.StaticData;
using Infrastructure.StaticData;
using UnityEngine;
using UI.Hudes;
using Window;
using Zenject;

namespace Infrastructure.Services.Factories.UIFactory
{
    public class UIFactory : Factory, IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;

        private Transform _uiRoot;

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
        public void CreateGameHud() => Instantiate(Path.GameHudPath).GetComponent<GameHud>();
        public MenuHud CreateMenuHud() => Instantiate(Path.MenuHudPath).GetComponent<MenuHud>();
        
        private PathResourcesStaticData Path => _staticData.PathResourcesConfig;
    }
}