using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System;
using System.Diagnostics;

namespace Assets.Scripts.Game
{
    public class CounterState : GameState
    {
        private DateTime _startTime;
        private int _lastSeconds;

        public CounterState(DateTime startTime) 
        {
            _startTime = startTime;
        }

        public override void OnEnter()
        {
            UIBus.OnCounterShown?.Invoke(true);
        }

        public override void OnExit()
        {
            UIBus.OnCounterShown?.Invoke(false);
        }

        public override void OnUpdate()
        {
            var remainingSeconds =  (_startTime - _startTime).Seconds;

            if (remainingSeconds == _lastSeconds)
                return;

            if (remainingSeconds <= 0) 
            {
                var cmd = new ChangeGameStateToCheckForGoalCmd();
                _stateMachine.ServerHub?.PerformCommand(cmd);
                _stateMachine.ServerHub?.SendCommandToAllClients(cmd);
                return;
            }

            _lastSeconds = remainingSeconds;
            UIBus.OnCounterUpdate?.Invoke(remainingSeconds);
        }
    }
}
