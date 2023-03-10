using Assets.Scripts.Network.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class GameBus
    {
        public static Action OnGameStarted;
        public static Action<ICommand> OnClientSendToServer;
    }
}
