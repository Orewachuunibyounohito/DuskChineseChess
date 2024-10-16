using System.Collections.Generic;
using ChineseChess.Chesses;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChineseChess.Controller
{
    public class MoveState : IState
    {   
        private ControllerStateMachine stateMachine;
        private HumanController controller;
        private NoneState noneState => stateMachine.NoneState;
        private PlayerControls.GameplayActions input;
        private CheckerBoardItem item;

        public MoveState(HumanController controller, ControllerStateMachine stateMachine){
            this.controller = controller;
            this.stateMachine = stateMachine;
            input = new PlayerControls().Gameplay;
        }

        public void Enter(){
            input.Enable();
            input.Click.performed += BehaviourConfirm;
            item.ShowReachable();
        }

        public void Exit(){
            item.HideReachable();
            input.Click.performed -= BehaviourConfirm;
            input.Disable();
        }

        private void BehaviourConfirm(InputAction.CallbackContext context){
            Ray mouseRay = Camera.main.ScreenPointToRay(input.Point.ReadValue<Vector2>());
            if(!Physics.Raycast(mouseRay, out RaycastHit rayHit, float.MaxValue, LayerMask.GetMask("Chess")))
            { return ; }

            if(rayHit.collider.CompareTag("PreviewChess")){
                var previewObject = rayHit.collider.gameObject;
                List<ICommand> commands = item.FindCommandsByObject(previewObject);
                controller.GetInvoker().AddCommands(commands);
                // item.OnChessDeselected();
                stateMachine.ChangeState(noneState);
            }
        }

        public void FrameUpdate(){}
        public void PhysicalUpdate(){}

        public void SetItem(CheckerBoardItem item) => this.item = item;
        public override string ToString() => "Move";
    }
}