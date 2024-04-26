using Infrastructure.Services.Factories.UIFactory;
using UnityEngine;
using UI.Window;
using Zenject;

namespace Infrastructure.Services.Window
{
    public class WindowService : IWindowService
    {
        private IUIFactory _uiFactory;

        [Inject]
        public void Constructor(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public RectTransform Open(WindowTypeId windowTypeId)
        {
            return _uiFactory.CrateWindow(windowTypeId);
        }
    }
}