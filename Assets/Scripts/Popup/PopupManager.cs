using System.Collections.Generic;
using UnityEngine;

namespace Army.Popup
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private RectTransform _container;
        //PRIVATE FIELDS
        private bool _isInitialized;
        private Dictionary<string, GameObject> _prefabsByName;

        private List<PopupView> _activePopupViews;

        // SINGLETON
        public static PopupManager Instance { get; private set; }
        
        // UNITY METHODS

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region IPopupManager implementation

        public void Initialize()
        {
            if (_isInitialized == false)
            {
                _isInitialized = true;

                _prefabsByName = new Dictionary<string, GameObject>();
                for (int i = 0; i < _prefabs.Length; i++)
                {
                    GameObject prefab = _prefabs[i];
                    _prefabsByName.Add(prefab.name, prefab);
                }

                _activePopupViews = new List<PopupView>();
            }
        }

        public void ShowPopup(string name)
        {
            if (_activePopupViews.Count > 0)
            {
                for (int i = 0; i < _activePopupViews.Count; i++)
                {
                    _activePopupViews[i].Hidden();
                }
            }

            GameObject popupPrefab = _prefabsByName[name];
            GameObject popupObject = Instantiate(popupPrefab, _container);
            PopupView popup = popupObject.GetComponent<PopupView>();

            popupObject.name = popupPrefab.name;
            popup.Initialize();
            popup.Open();
            _activePopupViews.Add(popup);
        }

        public T ShowPopup<T>() where T : PopupView
        {
            if (_activePopupViews.Count > 0)
            {
                for (int i = 0; i < _activePopupViews.Count; i++)
                {
                    _activePopupViews[i].Hidden();
                }
            }

            GameObject popupPrefab = _prefabsByName[typeof(T).Name];
            GameObject popupObject = Instantiate(popupPrefab, _container);
            T popup = popupObject.GetComponent<T>();

            popupObject.name = popupPrefab.name;
            popup.Initialize();
            popup.Open();
            _activePopupViews.Add(popup);

            return popup;
        }

        public void HideAllPopups()
        {
            if (_activePopupViews.Count > 0)
            {
                for (int i = 0; i < _activePopupViews.Count; i++)
                {
                    PopupView popup = _activePopupViews[i];
                    popup.Close();

                    Destroy(popup.gameObject);
                }

            }
            _activePopupViews.Clear();
        }

        public T GetActivePopup<T>() where T : PopupView
        {
            for (int i = 0; i < _activePopupViews.Count; i++)
            {
                if (_activePopupViews[i] is T)
                {
                    return (T)_activePopupViews[i];
                }
            }

            return null;
        }

        #endregion

    }
}

