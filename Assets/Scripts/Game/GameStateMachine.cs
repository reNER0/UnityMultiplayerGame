using Assets.Scripts.Network;
using System;
using Zenject;

namespace Assets.Scripts.Game
{
    public interface IGameStateMachine 
    {
        void ChangeState(GameState gameState);
    }

    public class GameStateMachine : IGameStateMachine, IInitializable, IDisposable, ITickable
    {
        private readonly IServerHub _serverHub;
        private readonly IServerRepository _serverRepository;

        private GameState _gameState = new WaitingForPlayersState(2);


        public IServerHub ServerHub => _serverHub;
        public IServerRepository ServerRepository => _serverRepository;

#if UNITY_SERVER
        public GameStateMachine(IServerHub serverHub, IServerRepository serverRepository) 
        {
            _serverHub = serverHub;
            _serverRepository = serverRepository;
        }
#endif


        public void Initialize()
        {
            GameBus.OnGameStateChanged += ChangeState;

            ChangeState(_gameState);
        }

        public void Tick()
        {
            _gameState.OnUpdate();
        }

        public void ChangeState(GameState newGameState) 
        {
            _gameState?.OnExit();

            newGameState.SetStateMachine(this);

            _gameState = newGameState;
            _gameState.OnEnter();
        }

        public void Dispose()
        {
            GameBus.OnGameStateChanged -= ChangeState;
        }
    }
}
