using Army.Game.UI;
using Army.Popup;
using Army.State;
using UnityEngine;

public class GamePlayGameState : AStateBase,
                                 IGameListener
{
    #region Private Members    

    IStateManager _stateManager;
    IUserDataManager _userDataManager;
    SceneReferences _sceneReferences;
    ResourceReferences _resourceReferences;

    private Camera _camera;

    private GameResources _gameResources;
    private bool _startedGame;

    private GameUIView _gameUIView;
    private GameView _gameView;
    private GameController _gameController;
  
    private CameraController _cameraController;

    private Level _level;

    //Pool
    #endregion

    //  CONSTRUCTION
    public GamePlayGameState(IStateManager stateManager,
                             IUserDataManager userDataManager,
                             SceneReferences sceneReferences,
                             ResourceReferences resourceReferences) : base(StateNames.Game)
    {
        _stateManager = stateManager;
        _userDataManager = userDataManager;
        _sceneReferences = sceneReferences;
        _resourceReferences = resourceReferences;
        _gameResources = _resourceReferences.GameResources;
    }

    #region State Methos
    public override void Activate()
    {
        Debug.Log("<color=green>GameplayGame State</color> OnActive");

        InitializeCamera();
        InitializeDriftGame();

        _gameView.Show();
        SubscribeEvents(); 
    }

    public override void Deactivate()
    {
        Debug.Log("<color=red>GameplayGame State</color> DeOnActive");

        UnsubscribeEvents();
        ClearScene();
    }

    public override void UpdateState()
    {
    }

    #endregion

    #region Private Methos
    private void InitializeDriftGame()
    {
        _camera = _sceneReferences.MainCam;
        if (_gameView == null)
        {
            InstantiateGameUI();
            InstantiateGameView();
            InstantiateGameController();
        }
    }
    private void InstantiateGameUI()
    {
        GameObject gameUIViewObject = GameObject.Instantiate(_resourceReferences.GameUIPrefab, _sceneReferences.UIViewContainer.transform);
        _gameUIView = gameUIViewObject.GetComponent<GameUIView>();
        _gameUIView.Show();
    }

    private void InstantiateGameView()
    {
        GameObject mainMenuObject = GameObject.Instantiate(_resourceReferences.GameViewPrefab, _sceneReferences.ViewContainer.transform);
        _gameView = mainMenuObject.GetComponent<GameView>();
        _gameView.Initialize(_gameController, Camera.main, _resourceReferences.GameResources);

        _gameController = mainMenuObject.GetComponent<GameController>();
    }

    private void InstantiateGameController()
    {       
        _gameController.Initialize(_gameView, _gameUIView, this, _resourceReferences.GameResources, _camera);
    }

    private void InitializeCamera()
    {
        _cameraController = _sceneReferences.MainCam.GetComponent<CameraController>();
    }

    private void SubscribeEvents()
    {
        GameEvents.OnStartGame += StartGameListener;
        GameEvents.OnEndGame += EndGameListener;
        GameEvents.OnClickGotoMenu += GotoMenu;
        GameEvents.OnClickLevelNext += LevelNext;
    }

    private void UnsubscribeEvents()
    {
        GameEvents.OnStartGame -= StartGameListener;
        GameEvents.OnEndGame -= EndGameListener;
        GameEvents.OnClickGotoMenu -= GotoMenu;
        GameEvents.OnClickLevelNext -= LevelNext;
    }

    private void ClearScene()
    {
        _gameUIView.UnloadLevel();
        _gameUIView.Hide();

        _gameController.Unload();
        PopupManager.Instance.HideAllPopups();
    }

    private void GotoMenu()
    {
        _stateManager.ChangeTransitionState(StateNames.Loading, StateNames.MainMenu);
    }

    private void LevelNext()
    {
        _gameController.Unload();
        _gameController.Load();
    }

    private void EndGameListener(bool success)
    {
        if (!_gameController.IsGameStarted)
            return;    

        _gameController.IsGameStarted = false;
        
        if (success)
        {            
            PopupManager.Instance.ShowPopup(PopupNames.LevelSuccessPopup);
        }
        else
        {              
            PopupManager.Instance.ShowPopup(PopupNames.LevelFailedPopup);
        }
    }

    private void StartGameListener()
    {

    }
    #endregion

  
}
