using TMPro;
using UnityEngine;

namespace Army.Game.UI
{
    public class LoadingUIView : MonoBehaviour
    {
        //  MEMBERS       
        [SerializeField] private TMP_Text _loadingText;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

    }
}