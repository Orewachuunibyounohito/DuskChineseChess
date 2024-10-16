using UnityEngine;

namespace ChineseChess.Chesses
{
    public class Guard : Chess
    {
        public override ChessType Type{ get => ChessType.士; }
        public override string DisplayName => Camp == ChessCamp.Black? $"{Type}" : "仕";

        public Guard(){
            movement = new(){
                { DirectionType.上左, new Vector2Int(-1, 1) },
                { DirectionType.上右, new Vector2Int(1, 1) },
                { DirectionType.下左, new Vector2Int(-1, -1) },
                { DirectionType.下右, new Vector2Int(1, -1) }
            };
        }

        protected override bool OutOfArea(CheckerBoard board, DirectionType direction, int steps){
            var newPosition = Position + movement[direction] * steps;
            return board.OutOfMainArea(newPosition);
        }

    }
}
