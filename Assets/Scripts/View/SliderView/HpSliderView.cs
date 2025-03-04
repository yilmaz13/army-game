using System.Text;
using TMPro;
using UnityEngine;

public class HpSliderView : SliderView
{
    [SerializeField] private TextMeshPro _valueText; 
    [SerializeField] private HealthBarColorConfig _healthBarColorConfig;
    
    private StringBuilder _stringBuilder = new StringBuilder();    

    public override void Initialize(float value, float maxValue, Team team)
    {   
        base.Initialize(value, maxValue, team);
        SetSliderColor(team);      
    }  
    
    private void SetSliderColor(Team team)
    {
        var teamColor =  _healthBarColorConfig.GetColor(team);
        _sliderInside.color = teamColor;
    }
     public override void UpdateValue(float value, float maxValue)
    {
        if (value == 0)
        {
            Hide();
        }
        else
        {                 
            SetSliderValue(value / maxValue);
            _stringBuilder.Clear();  
            _stringBuilder.Append(value);   
            _stringBuilder.Append("/");
            _stringBuilder.Append(maxValue);
            _valueText.text = _stringBuilder.ToString();          
        }           
    }
}
