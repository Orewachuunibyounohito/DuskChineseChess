using System;
using ChineseChess.Controller;
using Sirenix.OdinInspector;

namespace ChineseChess
{
    public class UserControlState : IState
    {
        private PlayerStateMachine stateMachine;
        private PerformState performState => stateMachine.PerformState;
        private Player player;
        [ShowInInspector]
        private IController controller;

        public UserControlState(PlayerStateMachine stateMachine, Player player, IController controller){
            this.stateMachine = stateMachine;
            this.player = player;
            this.controller = controller;
        }

        public void Enter(){
            player.OnMyVision();
            controller.Enable();
        }

        public void Exit(){
            controller.Disable();
        }

        public void FrameUpdate(){
            if(IsActionConfirm()){
                performState.SetAction(controller.GetInvoker());
                stateMachine.ChangeState(performState);
            }
        }

        public void PhysicalUpdate(){}

        private bool IsActionConfirm() => controller.ConfirmDestination();

        public override string ToString() => "UserControl";
    }
}
