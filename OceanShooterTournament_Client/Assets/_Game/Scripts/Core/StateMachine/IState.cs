namespace Core.StateMachine
{
    public interface IState
    {
        void EnterState();
        void Action();
        void ExitState();
    }
}