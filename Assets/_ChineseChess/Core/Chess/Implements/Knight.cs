using UnityEngine;

namespace ChineseChess.Chesses
{
    public class Knight : Chess
    {
        public override ChessType Type{ get => ChessType.馬; }
        public override string DisplayName => Camp == ChessCamp.Black? $"{Type}" : "傌";
        public override int VisionRadius => 2;

        public Knight(){
            movement = new(){
                { DirectionType.上左, new Vector2Int(-1, 2) },
                { DirectionType.上右, new Vector2Int(1, 2) },
                { DirectionType.下左, new Vector2Int(-1, -2) },
                { DirectionType.下右, new Vector2Int(1, -2) },
                { DirectionType.左上, new Vector2Int(-2, 1) },
                { DirectionType.左下, new Vector2Int(-2, -1) },
                { DirectionType.右上, new Vector2Int(2, 1) },
                { DirectionType.右下, new Vector2Int(2, -1) },
            };
        }
        
        protected override bool PathBlocked(CheckerBoard board, DirectionType direction, int steps){
            direction = direction switch{
                DirectionType.上左 or DirectionType.上右 => DirectionType.上,
                DirectionType.下左 or DirectionType.下右 => DirectionType.下,
                DirectionType.左上 or DirectionType.左下 => DirectionType.左,
                DirectionType.右上 or DirectionType.右下 => DirectionType.右,
                _ => throw new System.Exception("Unrecognized Direction.")
            };
            Vector2Int checkedPosition = Position + AroundOffset[direction] * steps;
            return board.HasChess(checkedPosition, Camp);
        }
    }
}
