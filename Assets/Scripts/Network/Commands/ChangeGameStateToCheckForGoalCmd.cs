using Assets.Scripts.Game;
using System;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class ChangeGameStateToCheckForGoalCmd : SerializableClass, ICommand
    {
        public void Execute()
        {
            GameBus.OnGameStateChanged?.Invoke(new CheckForGoalState());
        }
    }
}
