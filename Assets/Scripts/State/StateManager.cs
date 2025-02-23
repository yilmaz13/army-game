using System.Collections.Generic;

namespace Army.State
{
    public class StateManager : IStateManager
    {
        #region Public Members

        public string CurrentState { get; private set; }
        public string TransitionState { get; private set; }

        #endregion

        #region Private Members    

        private Dictionary<string, AStateBase> _gameStates;

        #endregion

        //  CONSTRUCTION
        public StateManager()
        {
            CurrentState = "";
            TransitionState = "";
            _gameStates = new Dictionary<string, AStateBase>();
        }

        #region Public Methos

        public void AddStates(AStateBase stateHandler)
        {
            _gameStates.Add(stateHandler.name, stateHandler);
        }

        public void ChangeState(string state)
        {
            if (string.IsNullOrEmpty(CurrentState) == false)
            {
                _gameStates[CurrentState].Deactivate();
            }

            string prevState = CurrentState;
            CurrentState = state;

            if (string.IsNullOrEmpty(CurrentState) == false)
            {
                _gameStates[CurrentState].Activate();
            }
        }

        public AStateBase GetCurrentState()
        {
            return _gameStates[CurrentState];
        }

        public void ChangeTransitionState(string state, string targetState)
        {
            if (string.IsNullOrEmpty(CurrentState) == false)
            {
                _gameStates[CurrentState].Deactivate();
            }

            string prevState = CurrentState;
            CurrentState = state;

            //TODO
            LoadingState _loadingState = (LoadingState)_gameStates[StateNames.Loading];
            _loadingState.SetTransitionState(targetState);

            if (string.IsNullOrEmpty(CurrentState) == false)
            {
                _gameStates[CurrentState].Activate();
            }
        }

        #endregion
    }
}