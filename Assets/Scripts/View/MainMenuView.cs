using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Army.Game.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _playGameButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        public void Show()
        {
            gameObject.SetActive(true);
            _playGameButton.onClick.AddListener(PlayGame);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _playGameButton.onClick.RemoveListener(PlayGame);
        }
        public void PlayGame()
        {
            GameEvents.ClickGotoGameScene();
        }

        public void DisplayLevelText(int levelAmount)
        {
            _levelText.text = levelAmount.ToString();
        }
    }
}
