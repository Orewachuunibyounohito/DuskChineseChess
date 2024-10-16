using System;

namespace ChineseChess.Chesses
{
    [Serializable]
    public class MovementContent
    {
        public DirectionType Direction;
        public int Steps;

        public MovementContent(DirectionType direction, int steps){
            Direction = direction;
            Steps = steps;
        }
    }
}
