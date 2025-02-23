using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Army.Popup
{
    public class SuccesedPopupView : PopupView
    {
        //  MEMBERS
        //      For Editor
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Button _nextButton;
        
        //      Private
        private bool isInputEnabled;

        //  METHODS
        override public void Open()
        {
            base.Open();
        }

        override protected void EnableInput()
        {
            if (isInputEnabled == false)
            {
                isInputEnabled = true;
                _nextButton.onClick.AddListener(OnNextButtonClick);              
            }
        }

        override protected void DisableInput()
        {
            if (isInputEnabled == true)
            {
                isInputEnabled = false;
                _nextButton.onClick.RemoveListener(OnNextButtonClick);              
            }
        }

        private void OnNextButtonClick()
        {
            GameEvents.ClickLevelNext();
        }
        
    }
}