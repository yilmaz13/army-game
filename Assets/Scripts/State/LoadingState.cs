using DG.Tweening;
using UnityEngine;
using Army.State;
using Army.Game.UI;

public class LoadingState : AStateBase
{
    private string _transitionState = StateNames.MainMenu;

    IStateManager _stateManager;
    SceneReferences _sceneReferences;
    ResourceReferences _resourceReferences;

    private LoadingUIView _loadingUIView;

    public LoadingState(IStateManager stateManager,
                        SceneReferences sceneReferences,
                        ResourceReferences resourceReferences) : base(StateNames.Loading)
    {
        _stateManager = stateManager;
        _sceneReferences = sceneReferences;
        _resourceReferences = resourceReferences;
    }

    public override void Activate()
    {
        Debug.Log("<color=green>Loading State</color> OnActive");

        if (_loadingUIView == null)
        {
            var _loadingUIViewObject = GameObject.Instantiate(_resourceReferences.LoadingUIView, _sceneReferences.UIViewContainer.transform);
            _loadingUIView = _loadingUIViewObject.GetComponent<LoadingUIView>();
        }

        _loadingUIView.Show();

        DOVirtual.DelayedCall(2, LoadingGame);
    }

    public override void Deactivate()
    {
        Debug.Log("<color=red>Loading State</color> Ondeactive");
    }

    public override void UpdateState()
    {
    }

    public void SetTransitionState(string transitionState)
    {
        _transitionState = transitionState;
    } 

    private void LoadingGame()
    {
        _loadingUIView.Close();
        _stateManager.ChangeState(_transitionState);
    }
}
