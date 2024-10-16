using System.Collections.Generic;
using UnityEngine;

namespace ChineseChess.Chesses
{
    public class Rook : Chess
    {
        public override ChessType Type{ get => ChessType.車; }
        public override string DisplayName => Camp == ChessCamp.Black? $"{Type}" : "俥";
        public override int VisionRadius => 2;

        public Rook(){
            movement = new(){
                {DirectionType.上, new Vector2Int(0, 1) },
                {DirectionType.下, new Vector2Int(0, -1) },
                {DirectionType.左, new Vector2Int(-1, 0) },
                {DirectionType.右, new Vector2Int(1, 0) },
            };
        }

        public override List<MovementContent> FindOutReachable(CheckerBoard board){
            List<MovementContent> reachable = new();
            foreach(var direction in movement.Keys){
                var steps = 1;
                var content = new MovementContent(direction, steps);
                while(!IsInvalidPosition(board, content.Direction, content.Steps)){
                    reachable.Add(content);
                    steps++;
                    content = new MovementContent(direction, steps);
                }
                
                if(board.IsEncounterOpponent(NextPosition(direction, steps), Camp)){
                    content = new MovementContent(direction, steps);
                    reachable.Add(content);
                }
            }
            this.reachable = reachable;
            return this.reachable;
        }
    }
}
