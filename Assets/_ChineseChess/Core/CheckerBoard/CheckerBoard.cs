using System;
using System.Collections.Generic;
using System.Linq;
using ChineseChess.Chesses;
using ChineseChess.Tools;
using Sirenix.Utilities;
using UnityEngine;

namespace ChineseChess
{
    public class CheckerBoard
    {   
        public delegate void ChessAddedEvent(CheckerBoard model, Chess chess, Vector2Int position);
        public delegate void ChessMovedEvent(Chess chess, Vector2Int position);
        public delegate void ChessMovedWithCommandEvent(Chess chess, Vector2Int position);
        public delegate void ChessDefeatedEvent(Player player, Chess defeatedChess);
        public delegate void PlayerWinEvent();
             
        private static Vector2Int borderPosition;

        private const int WIDTH = 9;
        private const int LENGTH = 10;
        private Dictionary<Vector2Int, Chess> _board;

        public event ChessAddedEvent ChessAdded;
        public event ChessMovedEvent ChessMoved;
        public event ChessMovedWithCommandEvent ChessMovedWithCommand;
        public event PlayerWinEvent PlayerWin;

        static CheckerBoard(){
            borderPosition = new Vector2Int(WIDTH - 1, LENGTH - 1);
        }

        public CheckerBoard(){
            _board = new();
        }

        public Chess FindChessByPosition(Vector2Int position) => _board.ContainsKey(position)? _board[position] : Chess.Empty;
        public Vector2Int FindPositionByChess(Chess chess) => _board.Keys.First((key) => _board[key] == chess);
        public (int width, int length) GetBoardSize() => (WIDTH, LENGTH);

        public void PrintBoard(){
            string result = "";
            for(int column = LENGTH-1; column >= 0; column--){
                result += "\n| ";
                for(int row = 0; row < WIDTH; row++){
                    var position = new Vector2Int(row, column);
                    result += FindChessByPosition(position).DisplayName;
                }
                result += " |\n";
            }
            Debug.Log(result);
        }

        public bool HasChess(Vector2Int chessPosition, ChessCamp camp){
            chessPosition = ChessPositionMappingTool.PositionMapping(chessPosition, camp);
            return !Chess.IsEmptyOrNull(FindChessByPosition(chessPosition));
        }

        public bool OutOfArea(Vector2Int chessPosition){
            return chessPosition.x >= WIDTH || chessPosition.x < 0
                || chessPosition.y >= LENGTH || chessPosition.y < 0;
        }
        public bool OutOfMainArea(Vector2Int chessPosition){
            return chessPosition.x > 5 || chessPosition.x < 3
                || chessPosition.y > 2 || chessPosition.y < 0;
        }
        public bool OutOfSelfArea(Vector2Int chessPosition){
            return chessPosition.x >= WIDTH || chessPosition.x < 0
                || chessPosition.y > LENGTH/2 || chessPosition.y < 0;
        }

        public bool IsEncounterOpponent(Vector2Int chessPosition, ChessCamp camp){
            var mappedPosition =  ChessPositionMappingTool.PositionMapping(chessPosition, camp);
            var chess = FindChessByPosition(mappedPosition);
            return !Chess.IsEmptyOrNull(chess) && chess.Camp != camp;
        }
        public bool IsComrades(Vector2Int chessPosition, ChessCamp camp){
            var mappedPosition = ChessPositionMappingTool.PositionMapping(chessPosition, camp);
            var chess = FindChessByPosition(mappedPosition);
            return !Chess.IsEmptyOrNull(chess) && chess.Camp == camp;
        }

        public void SetChess(Chess chess){
            Vector2Int mappedPosition = ChessPositionMappingTool.PositionMapping(chess);
            if(_board.ContainsKey(mappedPosition)){
                _board[mappedPosition] = chess;
            }else{
                _board.Add(mappedPosition, chess);
            }
            ChessAdded.Invoke(this, chess, mappedPosition);
        }


        public void AssignPlayers(Player red, Player black){
            red.GetAlives().ForEach((chess) => SetChess(chess));
            black.GetAlives().ForEach((chess) => SetChess(chess));
        }

        public Vector2Int BaseUpdateBoard(Chess chess, Vector2Int newPosition){
            Vector2Int mappedPosition = ChessPositionMappingTool.PositionMapping(chess.Position, chess.Camp);
            _board.Remove(mappedPosition);
            mappedPosition = ChessPositionMappingTool.PositionMapping(newPosition, chess.Camp);
            if(_board.ContainsKey(mappedPosition)){
                var hurtChess = _board[mappedPosition];
                if(hurtChess.Type == ChessType.將){
                    PlayerWin.Invoke();
                }
                
                hurtChess.DefeatedBy(chess);
            }
            _board[mappedPosition] = chess;
            return mappedPosition;
        }
        public void UpdateBoard(Chess chess, Vector2Int newPosition){
            var mappedPosition = BaseUpdateBoard(chess, newPosition);
            ChessMoved.Invoke(chess, mappedPosition);
        }

        public void UpdateBoardWithCommand(Chess chess, Vector2Int newPosition){
            var mappedPosition = BaseUpdateBoard(chess, newPosition);
            ChessMovedWithCommand.Invoke(chess, mappedPosition);
        }
    }
}
