using ChineseChess.Tools;
using ChuuniExtension.CountdownTools;
using UnityEngine;

namespace ChineseChess
{
    public class OpeningState : IState
    {
        private ChessStateMachine stateMachine;
        private bool isPrepared;
        private CheckerBoardView boardView;

        private Animator animator;

        private ICountdown debugCountdown;

        public OpeningState(ChessStateMachine stateMachine){
            this.stateMachine = stateMachine;
            debugCountdown = CountdownFactory.CustomGenerate(CountdownType.Interval, CountdownConfig.TimeConfig(3f));
        }
        public OpeningState(ChessStateMachine stateMachine, GameplayContent content) : this(stateMachine){
            boardView = content.BoardView;
        }

        public void Enter(){
            Reset();
            animator?.Play("Opening");
        }

        public void FrameUpdate(){
            debugCountdown.Update();
            if(debugCountdown.TimesUp){ isPrepared = true; }
            if(isPrepared){
                boardView.IntoPlayingState();
                var mappingOffset = ChessPositionMappingTool.GetOffset();
                mappingOffset.y = boardView.GetChessY();
                ChessPositionMappingTool.AdjustOffset(mappingOffset);
                stateMachine.ChangeState(stateMachine.PlayingState);
                return ;
            }
        }
        public void PhysicalUpdate(){}
        public void Exit(){}

        private void Reset() => isPrepared = false;

        public override string ToString() => "Opening";
    }
}
