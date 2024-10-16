using System.Collections.Generic;
using UnityEngine;

namespace ChineseChess.Chesses
{
    public class Artillery : Chess
    {
        public override ChessType Type{ get => ChessType.砲; }
        public override string DisplayName => Camp == ChessCamp.Black? $"{Type}" : "炮";
        public override int VisionRadius => 2;

        public Artillery(){
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
                steps++;
                if(TryFindReachableEnemy(board, direction, steps, out MovementContent found)){
                    reachable.Add(found);
                }
            }
            this.reachable = reachable;
            return this.reachable;
        }

        private bool TryFindReachableEnemy(CheckerBoard board, DirectionType direction, int startStep, out MovementContent content){
            content = default;
            var steps = startStep;
            while(!IsInvalidPosition(board, direction, steps)){
                steps++;
            }
            if(board.IsEncounterOpponent(NextPosition(direction, steps), Camp)){
                content = new MovementContent(direction, steps);
                return true;
            }
            return false;
        }
    }
}
