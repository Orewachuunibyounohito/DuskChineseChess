using System;
using ChineseChess.Chesses;
using Sirenix.OdinInspector;

namespace ChineseChess
{
    [Serializable]
    public class ChessCommandContent
    {
        [ShowInInspector]
        public Chess Chess;
        public CommandType Type;
        [ShowIf("IsMovement")]
        public MovementContent Movement;

        private bool IsMovement => Type == CommandType.Movement;
    }
}
