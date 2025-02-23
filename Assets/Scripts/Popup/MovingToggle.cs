using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MovingToggle : MonoBehaviour
{
    [SerializeField] private RectTransform _uiHandleRectTransform;
    [SerializeField] private Sprite disabledHandle, enabledHandle;

    private Image  _handleImage;

    private Toggle _toggle;

    private Vector2 _handlePosition;

    private bool isInitialized;

    public void Start()
    {
        Init();
    }
    public void Init()
    {
        if (isInitialized) return;

        isInitialized = true;
        _toggle = GetComponent<Toggle>();

        _handlePosition = _uiHandleRectTransform.anchoredPosition;
     
        _handleImage = _uiHandleRectTransform.GetComponent<Image>();

        _toggle.onValueChanged.AddListener(OnSwitch);
    
        _handleImage.sprite = _toggle.isOn ? enabledHandle : disabledHandle;

        if (_toggle.isOn)
            OnSwitch(true);
    }

    void OnSwitch(bool on)
    {
        _uiHandleRectTransform.DOAnchorPos(on ? _handlePosition * -1 : _handlePosition, .4f).SetEase(Ease.InOutBack);    
        _handleImage.sprite = on ? enabledHandle : disabledHandle;
    }

    void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
