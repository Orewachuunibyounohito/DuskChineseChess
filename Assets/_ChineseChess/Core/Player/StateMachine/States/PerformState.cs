using ChuuniExtension.CountdownTools;

namespace ChineseChess
{
    public class PerformState : IState
    {
        private const float EYECATCH_DURATION = 1.5f;
        private PlayerStateMachine stateMachine;
        private IState turnEndState => stateMachine.TurnEndState;
        private CommandInvoker invoker;
        private Player player;
        private bool countdownIsIdle = true;
        private ICountdown_Coroutine eyecatch;

        public PerformState(PlayerStateMachine stateMachine, Player player){
            this.stateMachine = stateMachine;
            this.player = player;
            eyecatch = new TimeCountdown_Coroutine(EYECATCH_DURATION);
        }

        public void Enter(){
            invoker.Execute();
            countdownIsIdle = true;
        }

        public void Exit(){
            player.OnActivateFogs();
            player.TurnAwayMyVision();
        }

        public void FrameUpdate(){
            if(!PerformEnd()){ return ; }
            if(countdownIsIdle){
                eyecatch.Start();
                countdownIsIdle = false;
            }
            if(!eyecatch.TimesUp){ return ; }
            eyecatch.Reset();
            stateMachine.ChangeState(turnEndState);
        }

        public void PhysicalUpdate(){
        }

        private bool PerformEnd() => invoker.IsDone();

        public void SetAction(CommandInvoker invoker) => this.invoker = invoker;

        public override string ToString() => "Perform";
    }
}
