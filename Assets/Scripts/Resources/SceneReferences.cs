using Army.Popup;
using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    //  MEMBERS
    public GameObject ViewContainer { get { return _viewContainer; } }
    public GameObject UIViewContainer { get { return _uiViewContainer; } }
    public PopupManager PopupManager { get { return _popupManager; } }

    public Camera MainCam { get { return _mainCam; } }

    //      From Editor
    [SerializeField] public GameObject _viewContainer;
    [SerializeField] public GameObject _uiViewContainer;
    [SerializeField] public PopupManager _popupManager;

    [SerializeField] private Camera _mainCam;
}
