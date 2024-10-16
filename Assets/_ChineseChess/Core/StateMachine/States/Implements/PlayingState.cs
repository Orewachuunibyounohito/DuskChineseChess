using System;
using System.Collections.Generic;
using ChuuniExtension.CountdownTools;
using ChuuniExtension.Loggers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChineseChess
{
    public class PlayingState : IState, IViewerContent
    {
        private ChessStateMachine stateMachine;
        private Player red;
        private Player black;
        [ShowInInspector]
        private Player currentPlayer;
        private Queue<Player> playOne;
        private bool IsGameOver => currentPlayer.IsWinner && ChessCommandInvoker.Instance.IsDone();

        public event Action<string, string> UpdateViewer;

        public PlayingState(ChessStateMachine stateMachine, Player red, Player black){
            this.stateMachine = stateMachine;
            this.red = red;
            this.black = black;
            playOne = new();
        }

        public PlayingState(ChessStateMachine stateMachine, GameplayContent content)
             : this(stateMachine, content.Red, content.Black){
            content.Board.PlayerWin += PlayerWin;
        }

        public void Enter(){
            Reset();
            AssignPlayOneQueue(red, black);
            currentPlayer = playOne.Dequeue();
            currentPlayer.TurnEnter();
            UpdateViewer?.Invoke("CurrentPlayer", $"{currentPlayer.Camp}");
        }

        public void Exit(){
        }

        public void FrameUpdate(){
            if(IsGameOver){
                stateMachine.ChangeState(stateMachine.EndingState);
                return ;
            }
            currentPlayer.FrameUpdate();
            if(currentPlayer.IsTurnEnd()){
                TurnToNextPlayer();
            }
        }

        public void PhysicalUpdate(){
        }

        private void Reset(){
            playOne = new();
        }

        private void AssignPlayOneQueue(params Player[] players){
            var debugCount = 0;
            var index = Random.Range(0, players.Length);
            var startIndex = index;
            do{
                debugCount++;
                playOne.Enqueue(players[index]);
                index++;
                if(index == players.Length){ index = 0; }
                if(debugCount > 100){ break; }
            }while(index != startIndex);
        }

        private void TurnToNextPlayer(){
            playOne.Enqueue(currentPlayer);
            currentPlayer = playOne.Dequeue();
            currentPlayer.TurnEnter();
            UpdateViewer?.Invoke($"{ChessViewerFieldName.CurrentPlayer}", $"{currentPlayer.Camp}");
        }

        private void PlayerWin(){
            currentPlayer.GetWin();
            stateMachine.EndingState.SetWinnerName(currentPlayer.Camp.ToString());
        }

        public override string ToString() => "Playing";
    }
}
