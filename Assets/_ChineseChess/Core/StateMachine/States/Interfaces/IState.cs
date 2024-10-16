namespace ChineseChess
{
    public interface IState
    {
        void Enter();
        void FrameUpdate();
        void PhysicalUpdate();
        void Exit();
    }
}
