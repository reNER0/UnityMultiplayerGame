using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.Network.Commands
{
    [Serializable]
    public class InitClientCmd : SerializableClass, ICommand
    {
        [SerializeField]
        private int _clientId;
        [SerializeField]
        private string _serverUnixStartupTime;

        public InitClientCmd(int clientId, string serverUnixStartupTime)
        {
            _clientId = clientId;
            _serverUnixStartupTime = serverUnixStartupTime;
        }

        public void Execute()
        {
            const string FMT = "O";
            DateTime now2 = DateTime.ParseExact(_serverUnixStartupTime, FMT, CultureInfo.InvariantCulture);

            NetworkSettings.SetServerStartupTime(now2);
            NetworkRepository.SetClientId(_clientId);
            Debug.Log($"Init cmd: {_clientId}");
        }
    }
}
