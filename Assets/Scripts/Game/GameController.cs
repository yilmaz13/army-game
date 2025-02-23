using Army.Game.UI;
using UnityEngine;

public class GameController : MonoBehaviour, IGameViewListener
{
    #region Private Members    

    private GameData _data;
    private GameView _view;
    private GameUIView _gameUIView;
    private IGameListener _listener;
    private GameResources _gameResources;   

    private Camera _camera;    
   
    public GameView View => _view;
    #endregion

    //  CONSTRUCTION
    public GameController()
    {
        _data = new GameData();     
    }

    public void Initialize(GameView view, GameUIView gameUIView, IGameListener listener, GameResources gameResources, Camera camera)
    {
        _listener = listener;
        _gameResources = gameResources;
        _gameUIView = gameUIView;
        _camera = camera;
        _view = view;
       
        SubscribeEvents();  
    }

    #region Private Methods      
   
    private void SubscribeEvents()
    {
    }

    private void UnsubscribeEvents()
    {
    }
    #endregion

    #region Public Methods    
    public void Load(Level _level)
    {
        _data.Load();
        _view.Create();   
    }

    public void Unload()
    {
        _data.Unload();
        _view.Clear();      
    }

    public void OnDestory()
    {
        UnsubscribeEvents();
    }
  
    #endregion
}
