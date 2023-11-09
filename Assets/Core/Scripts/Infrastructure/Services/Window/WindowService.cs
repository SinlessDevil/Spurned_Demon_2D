using Services.Factories.UIFactory;
using Window;
using Zenject;

namespace Services.Window
{
    public class WindowService : IWindowService
    {
        private IUIFactory _uiFactory;

        [Inject]
        public void Constructor(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowTypeId windowTypeId)
        {
            _uiFactory.CrateWindow(windowTypeId);
        }
    }
}