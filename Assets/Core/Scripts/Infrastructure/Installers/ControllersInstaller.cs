using GameController;
using Zenject;

namespace Infrastructure.Installers
{
    public class ControllersInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerMoveController>().AsSingle().NonLazy();
        }
    }
}