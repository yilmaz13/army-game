using UnityEngine;
using Army.State;
using Army.Game.UI;

public class MenuGameState : AStateBase
{
    IStateManager _stateManager;
    IUserDataManager _userDataManager;
    SceneReferences _sceneReferences;
    ResourceReferences _resourceReferences;
    private MainMenuView _mainMenuView;

    public MenuGameState(IStateManager stateManager,
                         IUserDataManager userDataManager,
                         SceneReferences sceneReferences,
                         ResourceReferences resourceReferences) : base(StateNames.MainMenu)
    {
        _stateManager = stateManager;
        _userDataManager = userDataManager;
        _sceneReferences = sceneReferences;
        _resourceReferences = resourceReferences;
    }

    public override void Activate()
    {
        Debug.Log("<color=green>MenuGame State</color> OnActive");

        if (_mainMenuView == null)
        {
            var _mainMenuViewObject = GameObject.Instantiate(_resourceReferences.MainMenuUIPrefab, _sceneReferences.UIViewContainer.transform);
            _mainMenuView = _mainMenuViewObject.transform.GetComponent<MainMenuView>();
        }

        var level = _userDataManager.CurrentLevel();

        _mainMenuView.Show();
        _mainMenuView.DisplayLevelText(level);

        GameEvents.OnClickGotoGameScene += OnClickGotoGameListener;
    }

    public override void Deactivate()
    {
        Debug.Log("<color=red>MenuGame State</color> OnDeactive");

        _mainMenuView.Hide();
        GameEvents.OnClickGotoGameScene -= OnClickGotoGameListener;
    }

    public override void UpdateState()
    {
    }

    public void OnClickGotoGameListener()
    {
        _stateManager.ChangeTransitionState(StateNames.Loading, StateNames.Game);
    }
}
