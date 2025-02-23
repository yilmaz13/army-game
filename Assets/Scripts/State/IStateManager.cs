namespace Army.State
{
    public interface IStateManager
    {
        //  MEMBERS
        string CurrentState { get; }

        //  METHODS
        void AddStates(AStateBase stateHandler);
        void ChangeState(string state);
        void ChangeTransitionState(string state, string targetState);
    }
}