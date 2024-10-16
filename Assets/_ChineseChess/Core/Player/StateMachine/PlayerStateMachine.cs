using System;
using ChineseChess.Controller;
using ChuuniExtension.Loggers;
using UnityEngine;

namespace ChineseChess
{
    public class PlayerStateMachine : StateMachine, IViewerContent
    {        
        [HideInInspector]
        public UserControlState UserControlState;
        [HideInInspector]
        public PerformState PerformState;
        [HideInInspector]
        public TurnEndState TurnEndState;
        [HideInInspector]
        public Player player;

        public event Action<string, string> UpdateViewer;

        public PlayerStateMachine(Player player, IController controller){
            this.player = player;
            UserControlState = new UserControlState(this, player, controller);
            PerformState = new PerformState(this, player);
            TurnEndState = new TurnEndState(this, player);
        }

        public void TurnStart(){
            player.OnActivateFogs();
            player.TurnToMyVision();
            ChangeState(UserControlState);
        }
        public void OnNewPlayer() => Initialize(TurnEndState);
        public bool IsTurnEnd() => currentState == TurnEndState && !TurnEndState.IsGameOver;

        public override void ChangeState(IState nextState){
            base.ChangeState(nextState);
            UpdateViewer?.Invoke($"{ChessViewerFieldName.PlayerState}", $"{currentState}");
        }
    }
}
