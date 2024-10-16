namespace ChineseChess.Chesses
{
    public class EmptyChess : Chess
    {
        public override ChessType Type => ChessType.無;

        public override void Move(DirectionType direction, int steps = 1){
            throw new System.NotImplementedException();
        }
    }
}
