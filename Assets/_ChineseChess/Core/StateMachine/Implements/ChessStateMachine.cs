using System;
using ChuuniExtension.Loggers;
using UnityEngine;

namespace ChineseChess
{
    [Serializable]
    public class ChessStateMachine : StateMachine, IViewerContent
    {
        [HideInInspector]
        public OpeningState OpeningState;
        [HideInInspector]
        public PlayingState PlayingState;
        [HideInInspector]
        public EndingState EndingState;

        public event Action<string, string> UpdateViewer;

        public ChessStateMachine(GameplayContent content){
            OpeningState = new OpeningState(this, content);
            PlayingState = new PlayingState(this, content);
            EndingState = new EndingState(this, content.FogView);
        }

        public void Opening() => Initialize(OpeningState);

        public override void ChangeState(IState nextState){
            base.ChangeState(nextState);
            UpdateViewer?.Invoke($"{ChessViewerFieldName.GameplayCurrentState}", $"{currentState}");
        }
    }
}