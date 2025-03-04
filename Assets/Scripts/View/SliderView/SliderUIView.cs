using UnityEngine;
using UnityEngine.UI;

public class SliderUIView : MonoBehaviour
{
    [SerializeField] protected Slider _slider;
   
    public void SetSliderValue(float ration)
    {
        _slider.value = ration;
    }  

    public virtual void Initialize(float value, float maxValue)
    {       
        SetSliderValue(1);      
        Show();
        UpdateValue(maxValue, maxValue);       
    }

    public virtual void UpdateValue(float value, float maxValue)
    {
        if (value == 0)
        {
            Hide();
        }
        else
        {                 
            SetSliderValue(value / maxValue);          
        }           
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
