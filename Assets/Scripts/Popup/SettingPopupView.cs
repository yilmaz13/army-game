using UnityEngine;
using UnityEngine.UI;

namespace Army.Popup
{
    public class SettingPopupView : PopupView
    {
        #region Fields

        [SerializeField] private Button _exitButton;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Toggle _vibrationToggle;
        [SerializeField] private Button _retryButton;

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
                _retryButton.onClick.AddListener(OnRetryButtonClick);
                _exitButton.onClick.AddListener(OnExitButtonClick);
                _musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged);
                _soundToggle.onValueChanged.AddListener(OnSoundToggleValueChanged);
                _vibrationToggle.onValueChanged.AddListener(OnVibrationToggleValueChanged);
                SettingsEvents.OnMusicEnabledChanged += OnMusicEnabledChanged;
                SettingsEvents.OnSoundEnabledChanged += OnSoundEnabledChanged;
                SettingsEvents.OnVibrationEnabledChanged += OnVibrationEnabledChanged;
            }
        }

        protected override void DisableInput()
        {
            if (isInputEnabled == true)
            {
                isInputEnabled = false;
                _retryButton.onClick.RemoveListener(OnRetryButtonClick);
                _exitButton.onClick.RemoveListener(OnExitButtonClick);
                _musicToggle.onValueChanged.RemoveListener(OnMusicToggleValueChanged);
                _soundToggle.onValueChanged.RemoveListener(OnSoundToggleValueChanged);
                _vibrationToggle.onValueChanged.RemoveListener(OnVibrationToggleValueChanged);
                SettingsEvents.OnMusicEnabledChanged -= OnMusicEnabledChanged;
                SettingsEvents.OnSoundEnabledChanged -= OnSoundEnabledChanged;
                SettingsEvents.OnVibrationEnabledChanged -= OnVibrationEnabledChanged;
            }
        }
        #endregion

        #region Private Methods
        private void OnRetryButtonClick()
        {
            GameEvents.ClickLevelRestart();
        }

        private void OnExitButtonClick()
        {
            PopupManager.Instance.HideAllPopups();
        }

        private void OnMusicToggleValueChanged(bool value)
        {
            SettingsEvents.ChangeMusicEnabled(value);
        }

        private void OnSoundToggleValueChanged(bool value)
        {
            SettingsEvents.ChangeSoundEnabled(value);
        }

        private void OnVibrationToggleValueChanged(bool value)
        {
            SettingsEvents.ChangeVibrationEnabled(value);
        }

        private void OnMusicEnabledChanged(bool value)
        {
            _musicToggle.SetIsOnWithoutNotify(value);
        }

        private void OnSoundEnabledChanged(bool value)
        {
            _soundToggle.SetIsOnWithoutNotify(value);
        }

        private void OnVibrationEnabledChanged(bool value)
        {
            _vibrationToggle.SetIsOnWithoutNotify(value);
        }


        #endregion
    }
}
