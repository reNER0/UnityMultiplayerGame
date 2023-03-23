using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class CounterState : GameState
    {
        private DateTime _startTime;
        private int _lastSeconds;
        private Car[] _cars;

        public CounterState(DateTime startTime) 
        {
            _startTime = startTime;
        }

        public override void OnEnter()
        {
            UIBus.OnCounterShown?.Invoke(true);

            _cars = UnityEngine.Object.FindObjectsOfType<Car>();
            SetCarsMovableState(false);

            if (_stateMachine.ServerHub == null)
                return;

            foreach (var car in _cars)
            {
                var carNetworkObject = NetworkRepository.GetNetworkObject(car.gameObject);
                var spawnTransform = _stateMachine.SpawnPoints.PlayerSpawnPoints[carNetworkObject.OwnerId];

                var moveCmd = new SyncRigidbodyCmd(
                    NetworkRepository.GetGameObjectsId(car.gameObject), 
                    spawnTransform.position, 
                    spawnTransform.rotation,
                    Vector3.zero,
                    Vector3.zero,
                    NetworkSettings.CurrentTick
                    );

                _stateMachine.ServerHub?.PerformCommand(moveCmd);
                _stateMachine.ServerHub?.SendCommandToAllClients(moveCmd);
            }
        }

        public override void OnExit()
        {
            UIBus.OnCounterShown?.Invoke(false);

            SetCarsMovableState(true);


        }

        public override void OnUpdate()
        {
            var remainingSeconds =  (_startTime - DateTime.Now).Seconds;


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

        public void SetCarsMovableState(bool movable) 
        {
            foreach(var car in _cars) 
            {
                car.SetMovable(movable);
            }
        }
    }
}
