using UnityEngine;

namespace Army.Game.UI
{
    public class GameView : ABaseUIView
    {
        #region Private Members

        private IGameViewListener _listener;
        private Camera _gameCamera;
        private GameResources _gameResources;

        #endregion

        #region Public Members
        public string CurrentState { get; set; }
        public string TransitionState { get; set; }
        public Transform ViewTransform => transform;

        #endregion

        public void Initialize(IGameViewListener listener, Camera gameCamera, GameResources gameResources)
        {
            _listener = listener;
            _gameCamera = gameCamera;
            _gameResources = gameResources;
        }

        public void Clear()
        {
        }

        public void Create()
        {
        }       
    }
}
