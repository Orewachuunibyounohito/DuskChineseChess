using UnityEngine;

namespace ChineseChess.Chesses
{
    public class General : Chess
    {
        public override ChessType Type => ChessType.將;
        public override string DisplayName => Camp == ChessCamp.Black? $"{Type}" : "帥";

        public General(){
            movement = new(){
                { DirectionType.上, new Vector2Int(0, 1) },
                { DirectionType.下, new Vector2Int(0, -1) },
                { DirectionType.左, new Vector2Int(1, 0) },
                { DirectionType.右, new Vector2Int(-1, 0) },
            };
        }
        
        protected override bool OutOfArea(CheckerBoard board, DirectionType direction, int steps){
            var newPosition = Position + movement[direction] * steps;
            return board.OutOfMainArea(newPosition);
        }

        protected override bool PathBlocked(CheckerBoard board, DirectionType direction, int steps){
            return false;
        }
    }
}
