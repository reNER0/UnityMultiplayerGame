using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;

namespace Assets.Scripts.Game
{
    public abstract class GameState : SerializableClass
    {
        protected GameStateMachine _stateMachine;

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();

        public void SetStateMachine(GameStateMachine gameStateMachine) 
        {
            _stateMachine = gameStateMachine;
        }
    }
}
