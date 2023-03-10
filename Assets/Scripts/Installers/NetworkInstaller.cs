using Assets.Scripts.Network;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class NetworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
#if UNITY_SERVER
            Container.BindInterfacesTo<ServerHub>()
                            .AsCached();

            Container.BindInterfacesTo<ServerRepository>()
                .AsCached();
#else
            Container.BindInterfacesTo<ClientHub>()
                            .AsCached();
#endif
        }
    }
}
