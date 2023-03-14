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
            // TODO : Setup spawn positions
            var spawnCmd = new SpawnCmd("Player", client.ClientId, Vector3.up * 3, Quaternion.identity);

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
