using Army.State;
using UnityEngine;

public class AppContext : MonoBehaviour
{
    #region Private Members   

    [Header("References")]
    [SerializeField] private SceneReferences _sceneReferences;
    [SerializeField] private ResourceReferences _resourceReferences;
    [SerializeField] private SettingsManager _settingsManager;

    private StateManager _stateManager;
    private UserDataManager _userDataManager;   

    #endregion

    #region Unity Members   

    void Start()
    {
        _stateManager = new StateManager();
        _userDataManager = new UserDataManager();

        _stateManager.AddStates(new LoadingState(_stateManager, _sceneReferences, _resourceReferences));
        _stateManager.AddStates(new GamePlayGameState(_stateManager, _userDataManager, _sceneReferences, _resourceReferences));
        _stateManager.AddStates(new MenuGameState(_stateManager, _userDataManager, _sceneReferences, _resourceReferences));

        _sceneReferences.PopupManager.Initialize();

        _stateManager.ChangeState(StateNames.Loading);
        _settingsManager.Initialize(_userDataManager);
    }

    void Update()
    {
        _stateManager.GetCurrentState().UpdateState();
    }

    private void OnDestroy()
    {
        _stateManager.GetCurrentState().Deactivate();
    }
    #endregion
}