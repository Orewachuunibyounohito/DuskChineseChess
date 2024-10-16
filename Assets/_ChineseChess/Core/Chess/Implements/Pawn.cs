using UnityEngine;

namespace ChineseChess.Chesses
{
    public class Pawn : Chess
    {
        public override ChessType Type{ get => ChessType.卒; }
        public override string DisplayName => Camp == ChessCamp.Black? $"{Type}" : "兵";

        public Pawn(){
            movement = new(){
                {DirectionType.上, new Vector2Int(0, 1) }
            };
        }

        // protected override bool PathBlocked(CheckerBoard board, DirectionType direction){
        //     return false;
        // }
        protected override bool PathBlocked(CheckerBoard board, DirectionType direction, int steps){
            return false;
        }
    }
}
