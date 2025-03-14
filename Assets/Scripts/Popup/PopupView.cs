using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Army.Popup
{
    public class PopupView : MonoBehaviour
    {
        //  MEMBERS
        //      For Editor
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _panel;

        //      Private
        private Color _backgroundColor;


        //  METHODS
        #region PopupView implementations

        public virtual void Initialize()
        {
            _backgroundColor = _background.color;
            _background.color = Color.clear;
            _panel.anchoredPosition = new Vector2(0, -2000);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);

            _background.DOColor(_backgroundColor, 0.5f)
                       .SetEase(Ease.OutQuad).SetUpdate(true);

            _panel.DOAnchorPos(Vector2.zero, 0.5f)
                  .SetEase(Ease.OutQuad).SetUpdate(true)
                  .OnComplete(() => { EnableInput(); });
        }

        public virtual void Hidden()
        {
            DisableInput();

            _background.DOColor(Color.clear, 0.5f)
                       .SetEase(Ease.OutQuad).SetUpdate(true);

            _panel.DOAnchorPos(new Vector2(0, -2000), 0.5f)
                  .SetEase(Ease.OutQuad).SetUpdate(true)
                  .OnComplete(() => { gameObject.SetActive(false); });
        }

        public virtual void Revealed()
        {
            gameObject.SetActive(true);


            _background.DOColor(_backgroundColor, 0.5f)
                       .SetEase(Ease.OutQuad).SetUpdate(true);

            _panel.DOAnchorPos(Vector2.zero, 0.5f)
                  .SetEase(Ease.OutQuad).SetUpdate(true)
                  .OnComplete(() => { EnableInput(); });
        }

        public virtual void Close()
        {
            DisableInput();


            _background.DOColor(Color.clear, 0.5f)
                       .SetEase(Ease.OutQuad).SetUpdate(true);

            _panel.DOAnchorPos(new Vector2(0, -2000), 0.5f)
                  .SetEase(Ease.OutQuad).SetUpdate(true)
                  .OnComplete(() => { Destroy(gameObject); });
        }

        #endregion

        private void OnDestroy()
        {
            DOTween.Kill(_background);
            DOTween.Kill(_panel);
        }

        virtual protected void EnableInput() { }
        virtual protected void DisableInput() { }
    }
}