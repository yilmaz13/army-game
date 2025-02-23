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
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void LoadLevel(int level)
        {
            _levelNameLabel.text = "Level" + level;
            _settingButton.onClick.AddListener(OnSettingsButtonClick);
        }
        public void UnloadLevel()
        {
            _levelNameLabel.text = "Level --";
            _settingButton.onClick.RemoveListener(OnSettingsButtonClick);
        }

        private void OnSettingsButtonClick()
        {
            
        }
    }
}
