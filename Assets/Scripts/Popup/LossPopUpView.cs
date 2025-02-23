using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Army.Popup
{
    public class LossPopUpView : PopupView
    {
        #region Fields
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        private bool isInputEnabled;
        #endregion

        #region Public Methods
        public override void Open()
        {
            base.Open();
        }
        #endregion

        #region Protected Methods
        protected override void EnableInput()
        {
            if (isInputEnabled == false)
            {
                isInputEnabled = true;
                _menuButton.onClick.AddListener(OnBackButtonClick);
                _restartButton.onClick.AddListener(OnRetryButtonClick);
            }
        }

        protected override void DisableInput()
        {
            if (isInputEnabled == true)
            {
                isInputEnabled = false;
                _menuButton.onClick.RemoveListener(OnBackButtonClick);
                _restartButton.onClick.RemoveListener(OnRetryButtonClick);
            }
        }
        #endregion

        #region Private Methods
        private void OnRetryButtonClick()
        {
            GameEvents.ClickLevelRestart();
        }

        private void OnBackButtonClick()
        {
            GameEvents.ClickGotoMenu();
        }
        #endregion
    }
}
