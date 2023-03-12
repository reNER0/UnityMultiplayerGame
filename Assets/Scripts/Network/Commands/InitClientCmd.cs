using System;
using UnityEngine;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class InitClientCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private int _clientId;

        public InitClientCmd(int clientId)
        {
            _clientId = clientId;
        }

        public void Execute()
        {
            NetworkRepository.SetClientId(_clientId);
            Debug.Log($"Init cmd: {_clientId}");
        }
    }
}
