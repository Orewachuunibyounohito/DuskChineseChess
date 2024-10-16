using ChineseChess.Chesses;
using ChineseChess.SO;
using ChuuniExtension.CountdownTools;
using UnityEngine;

namespace ChineseChess
{
    public class EndingState : IState
    {
        private static GameObject congratulationPrefab;

        private StateMachine stateMachine;
        private FogView fog;
        private bool isPrepared;

        private string winner;
        private Congratulation congratulation;
        private TimeCountdown_Coroutine countodwn;

        static EndingState(){
            congratulationPrefab = Resources.Load<PrefabsSettings>(ResourcesPath.PREFABS_SETTINGS)
                                                    .Ending;
        }

        public EndingState(StateMachine stateMachine, FogView fog){
            this.stateMachine = stateMachine;
            this.fog = fog;
        }

        public void Enter(){
            isPrepared = false;

            countodwn = new TimeCountdown_Coroutine(3f);
            congratulation = Object.Instantiate(congratulationPrefab, RootGroup.Object)
                                   .GetComponent<Congratulation>();
            congratulation.GenerateParticle();
            congratulation.SetWinnerName(winner);
            countodwn.Start();
            fog.LightUpAll();
        }

        public void Exit(){
            Debug.Log("Winner is ");
            stateMachine.ChangeState(new NothingState());
        }

        public void FrameUpdate(){
            if(isPrepared){
                return ;
            }

            if(countodwn.TimesUp){
                congratulation.Stop();
                isPrepared = true;
            }
        }

        public void PhysicalUpdate(){
        }

        public void SetWinnerName(string name) => winner = name;

        public override string ToString() => "Ending";
    }
}
