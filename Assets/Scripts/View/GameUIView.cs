using Army.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Army.Game.UI
{
    public class GameUIView : MonoBehaviour
    {

        [SerializeField] private Button _settingButton;
        [SerializeField] private TMP_Text _levelNameLabel;

        public void Show()
        {
            gameObject.SetActive(true);
            _settingButton.onClick.AddListener(OnSettingsButtonClick);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _settingButton.onClick.RemoveListener(OnSettingsButtonClick);
        }
        public void LoadLevel(int level)
        {
            _levelNameLabel.text = "Level" + level;           
        }
        public void UnloadLevel()
        {
            _levelNameLabel.text = "Level --";            
        }

        private void OnSettingsButtonClick()
        {
            PopupManager.Instance.ShowPopup(PopupNames.SettingPopup);
        }
    }
}
