using System;
using System.Collections.Generic;
using ChineseChess.Chesses;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChineseChess.Controller
{
    public class RampageState : IState
    {
        private ControllerStateMachine stateMachine;
        private HumanController controller;
        private PlayerControls.GameplayActions input;
        private CheckerBoardItem item;
        private ChessCommandInvoker invoker;

        public RampageState(HumanController controller, ControllerStateMachine stateMachine, ChessCommandInvoker invoker){
            this.controller = controller;
            this.stateMachine = stateMachine;
            input = new PlayerControls().Gameplay;
            this.invoker = invoker;
        }

        public void Enter(){
            input.Enable();
            item.ShowRampageDirection();
            input.Click.performed += BehaviourConfirm;
        }

        public void Exit(){
            input.Disable();
            input.Click.performed -= BehaviourConfirm;
            item.HideReachable();
        }

        public void FrameUpdate(){}
        public void PhysicalUpdate(){}

        private void BehaviourConfirm(InputAction.CallbackContext context){
            Ray mouseRay = Camera.main.ScreenPointToRay(input.Point.ReadValue<Vector2>());
            if(!Physics.Raycast(mouseRay, out RaycastHit rayHit, float.MaxValue, LayerMask.GetMask("Chess")))
            { return ; }

            if(rayHit.collider.CompareTag("PreviewChess")){
                var previewObject = rayHit.collider.gameObject;
                List<ICommand> command = item.FindCommandsByObject(previewObject);
                invoker.AddCommands(command);
                stateMachine.ChangeState(stateMachine.NoneState);
            }
        }

        public void SetItem(CheckerBoardItem item) => this.item = item;
        public override string ToString() => "Rampage";
    }
}