using System;
using System.Collections.Generic;
using ChineseChess.Chesses;

namespace ChineseChess
{
    public class CommandFactory
    {
        public static ICommand Generate(ChessCommandContent content){
            return content.Type switch{
                CommandType.Movement => new MovementCommand(content.Chess, content.Movement),
                _ => throw new Exception("Unrecognized CommandType!")
            };
        }

        public static ICommand GenerateMovement(MovementCommandContent content, CheckerBoardItem item){
            return new MovementCommand(content.Chess,
                                       content.Movement,
                                       item);
        }
        public static List<ICommand> GenerateMovements(MovementCommandContent content, CheckerBoardItem item){
            List<ICommand> commands = new();
            for(int step = 1; step <= content.Movement.Steps; step++){
                commands.Add(new MovementCommand(content.Chess,
                                                 new MovementContent(content.Movement.Direction, 1),
                                                 item));
            }
            return commands;
        }
    }

    public class MovementCommandContent
    {
        public Chess Chess;
        public MovementContent Movement;
        public MovementCommandContent(Chess chess, MovementContent movement){
            Chess = chess;
            Movement = movement;
        }
    }
}
