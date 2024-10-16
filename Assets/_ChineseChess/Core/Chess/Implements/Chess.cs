using System;
using System.Collections.Generic;
using ChineseChess.SO;
using ChineseChess.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChineseChess.Chesses
{
    [Serializable]
    public abstract class Chess
    {
        public static Dictionary<DirectionType, Vector2Int> AroundOffset;
        public static bool IsEmptyOrNull(Chess chess) => chess == null || chess == Empty;
        public static Chess Empty{ get; protected set; }

        public virtual ChessType Type{ get; }
        public virtual ChessCamp Camp{ get; set; }
        [ShowInInspector, ReadOnly]
        public virtual string DisplayName{ get => $"{Type}"; }
        [ShowInInspector, ReadOnly]
        public Vector2Int Position{ get; set; } = new Vector2Int(0, 0);
        public virtual int VisionRadius{ get; } = 1;

        protected Dictionary<DirectionType, Vector2Int> movement;
        protected List<MovementContent> reachable;

        [HideInInspector]
        public event Action<Chess, Vector2Int> Moved;
        public event Action<Chess, Vector2Int> MovedWithCommand;
        public event Action<List<Vector2Int>> LightUpSurrounding;
        public event Action<Chess> ChessDefeated;

        static Chess(){
            Empty = new EmptyChess();
            AroundOffset = new();
            Resources.Load<CheckerBoradSettings>(ResourcesPath.CHECKERBOARD_SETTINGS)
                     .AroundOffsets
                     .offsets
                     .ForEach((offset) => AroundOffset.Add(offset.direction, offset.vector));
        }

        public virtual void Move(DirectionType direction, int steps = 1){
            Vector2Int newPosition = Position + movement[direction] * steps;
            Moved.Invoke(this, newPosition);
            Position = newPosition;
            OnLightUpSurrounding();
        }
        public virtual void MoveWithCommand(DirectionType direction, int steps = 1){
            Vector2Int newPosition = Position + movement[direction] * steps;
            MovedWithCommand.Invoke(this, newPosition);
            Position = newPosition;
        }

        private void OnLightUpSurrounding(){
            List<Vector2Int> surrounding = ChessPositionMappingTool.FindSurroundingByChess(this);
            LightUpSurrounding.Invoke(surrounding);
        }

        public virtual void DefeatedBy(Chess attacker){
            ChessDefeated.Invoke(this);
        }

        public Vector2Int NextPosition(DirectionType direction, int steps = 1) => 
            Position + movement[direction] * steps;

        public virtual List<MovementContent> FindOutReachable(CheckerBoard board){
            List<MovementContent> reachable = new();
            foreach(var direction in movement.Keys){
                var content = new MovementContent(direction, 1);
                if (IsInvalidPosition(board, content.Direction, content.Steps)){ continue; }
                reachable.Add(content);
            }
            this.reachable = reachable;
            return this.reachable;
        }

        // Only Rook & Artillery
        public List<RampageCommand> FindOutRampageDirection(CheckerBoard model, CheckerBoardItem item){
            List<RampageCommand> rampageCommands = new();
            Dictionary<DirectionType, List<MovementContent>> movementContents = new();
            foreach(var direction in movement.Keys){ movementContents[direction] = new(); }
            foreach(var content in FindOutReachable(model)){
                movementContents[content.Direction].Add(content);
            }
            foreach(var contents in movementContents.Values){
                if(contents.Count == 0){ continue; }
                RampageCommand command = new RampageCommand(this, contents, item);
                rampageCommands.Add(command);
            }
            return rampageCommands;
        }

        protected bool IsInvalidPosition(CheckerBoard board, DirectionType direction, int steps = 1){
            return OutOfArea(board, direction, steps)
                || IsComrades(board, direction, steps)
                || PathBlocked(board, direction, steps);
        }

        protected virtual bool OutOfArea(CheckerBoard board, DirectionType direction, int steps){
            Vector2Int checkedPosition = Position + movement[direction] * steps;
            return board.OutOfArea(checkedPosition);
        }

        protected virtual bool IsComrades(CheckerBoard board, DirectionType direction, int steps){
            Vector2Int checkedPosition = Position + movement[direction] * steps;
            return board.IsComrades(checkedPosition, Camp);
        }

        protected virtual bool PathBlocked(CheckerBoard board, DirectionType direction, int steps){
            Vector2Int checkedPosition = Position + AroundOffset[direction] * steps;
            return board.HasChess(checkedPosition, Camp) ;
        }

        public override string ToString(){
            for(int x = (-VisionRadius); x <= (VisionRadius); x++){
                for(int y = (-VisionRadius); y <= VisionRadius; y++){
                    var newPosition = Position + new Vector2Int(x, y);
                }
            }
            return $"{DisplayName} : {Position}";
        }

        public void ShowMovement(){
            string log = "";
            foreach(var direction in movement.Keys){
                log += $"{direction}, ";
            }
            log = log.Remove(log.Length-2);
            Debug.Log(log);
        }
        public void ShowReachable(){
            string log = "";
            foreach(var direction in movement.Keys){
                log += $"{direction}, ";
            }
            log = log.Remove(log.Length-2);
            Debug.Log(log);
        }
    }
}
