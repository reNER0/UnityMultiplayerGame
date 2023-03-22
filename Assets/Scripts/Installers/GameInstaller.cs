using Assets.Scripts.Game;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private SpawnPoints spawnPoints;

        public override void InstallBindings()
        {
            Container.Bind<SpawnPoints>()
                .FromInstance(spawnPoints)
                .AsCached();

            Container.BindInterfacesTo<GameStateMachine>()
                            .AsCached();
        }
    }
}
