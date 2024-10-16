using System;
using ChineseChess.Chesses;
using ChuuniExtension.Loggers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChineseChess.Controller
{
    [Serializable]
    public class HumanController : IController, IViewerContent
    {
        private PlayerControls.GameplayActions input;
        private Player owner;
        private CheckerBoardItem currentChess;
        private ChessCommandInvoker invoker;
        [ShowInInspector]
        private ControllerStateMachine stateMachine;
        private BehaviourMenu behaviourMenu;

        public event Action<string, string> UpdateViewer;

        public HumanController(Player player){
            input = new PlayerControls().Gameplay;
            owner = player;
            if(ChessCommandInvoker.IsNull()){
                invoker = new GameObject("ChessCommandInvoker").AddComponent<ChessCommandInvoker>();
            }else{
                invoker = ChessCommandInvoker.Instance;
            }
            stateMachine = new ControllerStateMachine(this, invoker);
            behaviourMenu = new BehaviourMenu(this);
        }

        public void Enable(){
            input.Enable();
            input.Click.performed += SelectChess;
            input.Cancel.performed += CloseOperationMenu;
        }

        public void Disable(){
            input.Click.performed -= SelectChess;
            input.Cancel.performed -= CloseOperationMenu;
            input.Disable();
        }

        public bool ConfirmDestination() => invoker == default? false : invoker.IsConfirm();

        public void SelectChess(InputAction.CallbackContext callback){
            var screenPoint = input.Point.ReadValue<Vector2>();
            Ray mouseRay = Camera.main.ScreenPointToRay(screenPoint);
            if(!Physics.Raycast(mouseRay, out RaycastHit rayHit, float.MaxValue, LayerMask.GetMask("Chess")))
            { return ; }

            if(rayHit.collider.CompareTag("Chess")){
                Deselect();
                var focusedItem = rayHit.collider.gameObject.GetComponent<CheckerBoardItem>();
                if(!owner.Contains(focusedItem.Chess)){ return ; }
                currentChess = focusedItem;
                UpdateViewer?.Invoke($"{ChessViewerFieldName.SelectedChess}", currentChess.GetName());
                OpenOpreationMenu(currentChess.Chess, screenPoint);
                Disable();
            }
        }

        private void OpenOpreationMenu(Chess chess, Vector2 position){
            behaviourMenu.OpenMenu(chess.Type, position);
        }
        
        private void CloseOperationMenu(InputAction.CallbackContext context) => behaviourMenu.CloseMenu();

        public CommandInvoker GetInvoker() => invoker;

        private void Deselect(){
            if(currentChess != null && currentChess != default){
                currentChess?.OnChessDeselected();
            }
            currentChess = default;
            UpdateViewer?.Invoke($"{ChessViewerFieldName.SelectedChess}", Chess.Empty.DisplayName);
        }

        public void ToMoveMode(){
            stateMachine.MoveState.SetItem(currentChess);
            stateMachine.ChangeState(stateMachine.MoveState);
        }

        public void ToRampageMode(){
            stateMachine.RampageState.SetItem(currentChess);
            stateMachine.ChangeState(stateMachine.RampageState);
        }

        public void RegisterViewerUpdate(ChessContentViewer viewer){
            UpdateViewer += viewer.UpdateField;
            stateMachine.UpdateViewer += viewer.UpdateField;
        }
    }
}
