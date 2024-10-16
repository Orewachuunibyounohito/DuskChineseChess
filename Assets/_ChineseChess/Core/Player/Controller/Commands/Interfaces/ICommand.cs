namespace ChineseChess
{
    public interface ICommand
    {
        bool IsFinished{ get; set; }

        void Execute();
    }
}
