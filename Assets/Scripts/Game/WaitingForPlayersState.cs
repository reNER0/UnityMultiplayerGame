using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class WaitingForPlayersState : GameState
    {
        private int _playersCount;

        public WaitingForPlayersState(int playersCount) 
        {
            _playersCount = playersCount;
        }

        public override void OnEnter()
        {
            NetworkBus.OnClientConnected += CreateNetworkObjects;

        }

        public override void OnExit()
        {
            NetworkBus.OnClientConnected -= CreateNetworkObjects;
        }

        public override void OnUpdate()
        {

        }

        private void CreateNetworkObjects(NetworkClient client)
        {
            var spawnTransform = _stateMachine.SpawnPoints.PlayerSpawnPoints[client.ClientId];
            var spawnCmd = new SpawnCmd("Player", client.ClientId, spawnTransform.position, spawnTransform.rotation);

            _stateMachine.ServerHub?.PerformCommand(spawnCmd);
            _stateMachine.ServerHub?.SendCommandToAllClients(spawnCmd);

            CheckForPlayersCount();
        }

        private void CheckForPlayersCount()
        {
            if (_stateMachine.ServerRepository?.GetClients().Length < _playersCount)
                return;

            var cmd = new ChangeGameStateToCounterCmd(DateTime.Now.AddSeconds(5));
            _stateMachine.ServerHub?.PerformCommand(cmd);
            _stateMachine.ServerHub?.SendCommandToAllClients(cmd);
        }
    }
}
