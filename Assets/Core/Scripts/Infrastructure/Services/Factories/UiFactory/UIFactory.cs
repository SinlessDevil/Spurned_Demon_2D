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
        private const string UiRootPath = "UI/UiRoot";
        private const string GameHudPath = "UI/Hud/GameHud";
        private const string MenuHudPath = "UI/Hud/MenuHud";

        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;

        private Transform _uiRoot;

        public UIFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }

        public void CreateUiRoot() => _uiRoot = Instantiate(UiRootPath).transform;

        public void CreateGameHud()
        {
            throw new System.NotImplementedException();
        }
        
        public MenuHud CreateMenuHud() => Instantiate(MenuHudPath).GetComponent<MenuHud>();

        public RectTransform CrateWindow(WindowTypeId windowTypeId)
        {
            WindowConfig config = _staticData.ForWindow(windowTypeId);
            GameObject window = Instantiate(config.Prefab, _uiRoot);
            return window.GetComponent<RectTransform>();
        }
    }
}