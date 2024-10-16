using UnityEngine;

namespace ChineseChess.Chesses
{
    public class Bishop : Chess
    {
        public override ChessType Type{ get => ChessType.象; }
        public override string DisplayName => Camp == ChessCamp.Black? $"{Type}" : "相";

        public Bishop(){
            movement = new(){
                { DirectionType.上左, new Vector2Int(-2, 2) },
                { DirectionType.上右, new Vector2Int(2, 2) },
                { DirectionType.下左, new Vector2Int(-2, -2) },
                { DirectionType.下右, new Vector2Int(2, -2) },
            };
        }

        protected override bool OutOfArea(CheckerBoard board, DirectionType direction, int steps){
            var newPosition = Position + movement[direction] * steps;
            return board.OutOfSelfArea(newPosition);
        }
    }
}
