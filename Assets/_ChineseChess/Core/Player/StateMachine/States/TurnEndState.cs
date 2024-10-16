using UnityEngine;

namespace ChineseChess
{
    public class TurnEndState : IState
    {
        private StateMachine stateMachine;
        private Player player;

        public bool IsGameOver => player.IsWinner;

        public TurnEndState(PlayerStateMachine stateMachine, Player player){
            this.stateMachine = stateMachine;
            this.player = player;
        }

        public void Enter(){}
        public void Exit(){}
        public void FrameUpdate(){}
        public void PhysicalUpdate(){}
        
        public override string ToString() => "TurnEnd";
    }
}
