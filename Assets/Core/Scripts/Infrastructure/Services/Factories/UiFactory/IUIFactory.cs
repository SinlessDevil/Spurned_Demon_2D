using UI.Hudes;
using UnityEngine;
using UI.Window;

namespace Infrastructure.Services.Factories.UIFactory
{
    public interface IUIFactory
    {
        void CreateUiRoot();
        void CreateGameHud();
        MenuHud CreateMenuHud();
        RectTransform CrateWindow(WindowTypeId windowTypeId);
    }
}