using System;
using System.Collections.Generic;
using ChineseChess.Chesses;
using ChuuniExtension.CountdownTools;
using UnityEngine;

namespace ChineseChess
{
    [Serializable]
    public class RampageCommand : ICommand
    {
        private Chess chess;
        [SerializeField]
        public DirectionType Direction;
        private List<MovementCommand> movementCommands;
        private CommandInvoker invoker;

        public bool IsFinished { get; set; } = false;

        public RampageCommand(Chess chess, List<MovementContent> movements, CheckerBoardItem item){
            this.chess = chess;
            Direction = movements[0].Direction;
            movementCommands = new List<MovementCommand>();
            int prevLocation = 0;
            int currStep;
            foreach(var content in movements)
            {
                currStep = content.Steps - prevLocation;
                prevLocation = content.Steps;
                if (currStep == 1) { continue; }

                movementCommands.Add(GenerateMovementCommandTemporaryPosition(content, currStep, prevLocation, item));
                movementCommands.Add(GenerateAttackCommand(content, currStep, item));
            }
            bool NoStop = movementCommands.Count == 0;
            if (NoStop){
                int lastIndex = movements.Count-1;
                movementCommands.Add((MovementCommand)CommandFactory.GenerateMovement(
                    new MovementCommandContent(this.chess, movements[lastIndex]), item
                ));
            }

            invoker = ChessCommandInvoker.Instance;
        }

        public void Execute(){
            invoker.AddCommands(movementCommands);
            IsFinished = true;
            invoker.Execute();
        }

        private MovementCommand GenerateMovementCommandTemporaryPosition(MovementContent content, int currStep, int prevLocation, CheckerBoardItem item){
            var adjustedContent = new MovementContent(content.Direction, prevLocation - currStep);
            return (MovementCommand)CommandFactory.GenerateMovement(
                new MovementCommandContent(chess, adjustedContent), item
            );
        }

        private MovementCommand GenerateAttackCommand(MovementContent content, int currStep, CheckerBoardItem item){
            var adjustedContent = new MovementContent(content.Direction, currStep);
            return (MovementCommand)CommandFactory.GenerateMovement(
                new MovementCommandContent(chess, adjustedContent), item
            );
        }
    }
}
