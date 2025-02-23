using System.Collections.Generic;
using UnityEngine;

namespace Army.Popup
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private RectTransform _container;
        //      Private
        private bool _isInitialized;
        private Dictionary<string, GameObject> _prefabsByName;

        private List<PopupView> _activePopupViews;

        //      Singleton instance
        public static PopupManager Instance { get; private set; }
        //      Unity Methods

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

        #endregion

    }
}

