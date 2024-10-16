using System;
using System.Collections;
using ChineseChess.Chesses;
using ChuuniExtension.CountdownTools;
using UnityEngine;

namespace ChineseChess
{
    [Serializable]
    public class MovementCommand : ICommand
    {
        private Chess chess;
        [SerializeField]
        private MovementContent movement;

        private CheckerBoardItem item;

        public bool IsFinished { get; set; }

        public MovementCommand(Chess chess, MovementContent movement){
            this.chess = chess;
            this.movement = movement;
        }
        public MovementCommand(MovementCommandContent content) : this(content.Chess, content.Movement){}

        public MovementCommand(Chess chess, MovementContent movement, CheckerBoardItem item){
            this.chess = chess;
            this.movement = movement;
            this.item = item;
        }

        public void Execute(){
            item.gameObject.AddComponent<ChessMoving>()
                           .SetCommand(this);
            chess.MoveWithCommand(movement.Direction, movement.Steps);
        }
    }
}
