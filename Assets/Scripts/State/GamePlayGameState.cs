using Army.Game.UI;
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
            InstantiateDriftController();
        }
    }
    private void InstantiateGameUI()
    {
        GameObject gameUIViewObject = GameObject.Instantiate(_resourceReferences.GameUIPrefab, _sceneReferences.UIViewContainer.transform);
        _gameUIView = gameUIViewObject.GetComponent<GameUIView>();
    }

    private void InstantiateGameView()
    {
        GameObject mainMenuObject = GameObject.Instantiate(_resourceReferences.GameViewPrefab, _sceneReferences.ViewContainer.transform);
        _gameView = mainMenuObject.GetComponent<GameView>();
        _gameView.Initialize(_gameController, Camera.main, _resourceReferences.GameResources);
    }

    private void InstantiateDriftController()
    {
        _gameController = new GameController();
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
    }

    private void UnsubscribeEvents()
    {
        GameEvents.OnStartGame -= StartGameListener;
        GameEvents.OnEndGame -= EndGameListener;
        GameEvents.OnClickGotoMenu -= GotoMenu;
    }

    private void GotoMenu()
    {
        _stateManager.ChangeTransitionState(StateNames.Loading, StateNames.MainMenu);
    }

    private void EndGameListener(bool success)
    {
        if (!_startedGame)
            return;

        _startedGame = false;

        if (success)
        {
            //LevelSuccess           
        }
        else
        {
            //LevelFail           
        }
    }

    private void StartGameListener()
    {

    }
    #endregion

  
}
