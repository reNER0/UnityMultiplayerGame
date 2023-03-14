using Assets.Scripts.Game;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameStateMachine>()
                            .AsCached();
        }
    }
}
