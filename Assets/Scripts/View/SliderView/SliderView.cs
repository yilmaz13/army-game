using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SliderView : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _sliderInside;
    [SerializeField] protected SpriteRenderer _sliderOutside;
   
    public void SetSliderValue(float ration)
    {
        var spriteSize = _sliderInside.sprite.rect.size;        
        _sliderInside.size = new Vector2(spriteSize.x * 0.01f * ration, _sliderInside.size.y);        
    }  

    public virtual void Initialize(float value, float maxValue, Team team)
    {       
        SetSliderValue(1);     
        Show();
        UpdateValue(maxValue, maxValue);      

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

    public void TrackParent(Vector3 vector3)
    {
        transform.position = vector3;
    }

    public void LookAtPosition(Transform pos)
    {
        if (pos == null)
        {
            Debug.LogWarning("SliderView: target is null.");
            return;
        }

        var lookDirection = pos.transform.position;
        lookDirection.y = transform.position.y;
        transform.LookAt(lookDirection, Vector3.up);
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
