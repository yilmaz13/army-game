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

        [SerializeField] private XpSliderUIView _blueArmyXpSlider;     

        public void SetupBlueArmyStatPanel(ArmyController army)
        {
            if (army != null && _blueArmyXpSlider != null)
            {
                _blueArmyXpSlider.SetTrackedArmy(army);
            }
        }

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
            _levelNameLabel.text = "Chapter" + level;           
        }
        public void UnloadLevel()
        {
            _levelNameLabel.text = "Chapter --";            
        }

        private void OnSettingsButtonClick()
        {
            PopupManager.Instance.ShowPopup(PopupNames.SettingPopup);
        }

    }
}
