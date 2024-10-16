using System;
using ChuuniExtension.Loggers;
using UnityEngine;

namespace ChineseChess.Controller
{
    public class ControllerStateMachine : StateMachine, IViewerContent
    {
        [HideInInspector]
        public NoneState NoneState;
        [HideInInspector]
        public MoveState MoveState;
        [HideInInspector]
        public RampageState RampageState;

        public ControllerStateMachine(HumanController controller, ChessCommandInvoker invoker){
            NoneState = new NoneState();
            MoveState = new MoveState(controller, this);
            RampageState = new RampageState(controller, this, invoker);
            Initialize(NoneState);
        }

        public event Action<string, string> UpdateViewer;

        public override void ChangeState(IState nextState)
        {
            base.ChangeState(nextState);
            UpdateViewer?.Invoke($"{ChessViewerFieldName.CotrollerCurrentState}", $"{nextState}");
        }
    }
}