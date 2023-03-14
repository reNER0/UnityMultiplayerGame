using Assets.Scripts.Game;
using Assets.Scripts.Network.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject.Asteroids;

namespace Assets.Scripts
{
    public static class GameBus
    {
        public static Action OnGameStarted;
        public static Action<GameState> OnGameStateChanged;
    }
}
