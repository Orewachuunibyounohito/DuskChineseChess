using System;
using Sirenix.OdinInspector;

namespace ChineseChess
{
    [Serializable]
    public abstract class StateMachine
    {
        [ShowInInspector]
        protected IState currentState;

        public void FrameUpdate() => currentState.FrameUpdate();
        public void PhysicalUpdate() => currentState.PhysicalUpdate();

        public void Initialize(IState initialState){
            currentState = initialState;
            currentState.Enter();
        }
        public virtual void ChangeState(IState nextState){
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
        }
    }
}
