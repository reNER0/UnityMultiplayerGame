using Assets.Scripts.Network;
using Assets.Scripts.Network.Commands;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class CheckForGoalState : GameState
    {
        private Goal[] _goals;

        public override void OnEnter()
        {
            _goals = Object.FindObjectsOfType<Goal>();

            foreach (var goal in _goals) 
            {
                goal.OnGoal += Goal;
            }

            if (_stateMachine.ServerHub == null)
                return;

            // TODO : Find better way to deal with it
            var ballSpawnPoint = _stateMachine.SpawnPoints.BallSpawnPoint;
            var spawnBallCmd = new SpawnCmd("Ball", NetworkRepository.CurrentCliendId, ballSpawnPoint.position, ballSpawnPoint.rotation);
            _stateMachine.ServerHub?.PerformCommand(spawnBallCmd);
            _stateMachine.ServerHub?.SendCommandToAllClients(spawnBallCmd);
        }

        public override void OnExit()
        {
            foreach (var goal in _goals)
            {
                goal.OnGoal -= Goal;
            }

            if (_stateMachine.ServerHub == null)
                return;

            // TODO : Find better way to deal with it
            var ballId = NetworkRepository.NetworkObjectById.First(x => x.Value.GameObject.tag == "Ball").Key;
            var destroyBallCmd = new DestroyCmd(ballId);
            _stateMachine.ServerHub?.PerformCommand(destroyBallCmd);
            _stateMachine.ServerHub?.SendCommandToAllClients(destroyBallCmd);
        }

        public override void OnUpdate()
        {

        }

        private void Goal() 
        {
            var cmd = new ChangeGameStateToCounterCmd(System.DateTime.Now.AddSeconds(3));
            _stateMachine.ServerHub?.PerformCommand(cmd);
            _stateMachine.ServerHub?.SendCommandToAllClients(cmd);
        }
    }
}
