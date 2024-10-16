using System;
using System.Collections.Generic;
using ChineseChess.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ChineseChess.Chesses
{
    public class CheckerBoardItem : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private Vector2Int chessPosition;
        private CheckerBoard board;
        private CheckerBoardView boardView;
        public Chess Chess => board.FindChessByPosition(chessPosition);
        private Dictionary<GameObject, ICommand> ObjectCommandPairs;
        private Dictionary<GameObject, List<ICommand>> ObjectCommandsPairs;

        public UnityAction<CheckerBoard, Chess> ChessSelected;
        public UnityAction ChessDeselected;

        public void Awake(){
            ObjectCommandPairs = new Dictionary<GameObject, ICommand>();
            ObjectCommandsPairs = new();
        }

        public CheckerBoardItem SetBoard(CheckerBoard board){
            this.board = board;
            return this;
        }
        public CheckerBoardItem SetBoardView(CheckerBoardView boardView){
            this.boardView = boardView;
            return this;
        }
        public CheckerBoardItem SetPosition(Vector2Int chessPosition){
            this.chessPosition = chessPosition;
            return this;
        }
        public string GetName() => Chess.DisplayName;

        public void OnChessSelected(){
            ChessSelected?.Invoke(board, Chess);
        }

        public void ShowReachable(){
            boardView.ShowReachable(board, Chess);
        }
        public void HideReachable(){
            boardView.HideReachable();
            ObjectCommandClear();
        }
        public void ShowRampageDirection(){
            boardView.ShowRampageDirection(board, Chess);
        }
        public void HideRampageDirection(){
            HideReachable();
        }

        public void OnChessDeselected(){
            ChessDeselected?.Invoke();
        }

        public void OnMoved(Chess chess, Vector2Int destination) => chessPosition = ChessPositionMappingTool.PositionMapping(destination, chess.Camp);

        public void SetObjectCommand(Dictionary<GameObject, ICommand> pairs) => ObjectCommandPairs = pairs;
        public void AddObjectCommand(GameObject previewObject, ICommand command){
            ObjectCommandPairs.Add(previewObject, command);
        }
        public void AddObjectCommands(GameObject previewObject, List<ICommand> commands){
            ObjectCommandsPairs.Add(previewObject, commands);
        }
        public ICommand FindCommandByObject(GameObject chessObject) => ObjectCommandPairs[chessObject];
        public List<ICommand> FindCommandsByObject(GameObject chessObject) => ObjectCommandsPairs[chessObject];

        public void ObjectCommandClear(){
            ObjectCommandPairs.Clear();
            ObjectCommandsPairs.Clear();    
        }

    }
}
