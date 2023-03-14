using Assets.Scripts.Game;
using System;
using UnityEngine;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class ChangeGameStateToCounterCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private DateTime _dateTime;

        public ChangeGameStateToCounterCmd(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public void Execute()
        {
            GameBus.OnGameStateChanged?.Invoke(new CounterState(_dateTime));
        }
    }
}
