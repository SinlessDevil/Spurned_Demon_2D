using UI.Hudes;
using UnityEngine;
using UI.Window;

namespace Infrastructure.Services.Factories.UIFactory
{
    public interface IUIFactory
    {
        
        public GameHud GameHud { get; }
        public MenuHud MenuHud { get;}
        void CreateUiRoot();
        GameHud CreateGameHud();
        MenuHud CreateMenuHud();
        RectTransform CrateWindow(WindowTypeId windowTypeId);
    }
}