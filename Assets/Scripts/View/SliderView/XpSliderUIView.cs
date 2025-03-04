using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpSliderUIView : SliderUIView
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _xpText;    
    [SerializeField] private Transform _handleSlider;

    [SerializeField] private Tween _handleTween;
    private ArmyController _trackedArmy;

    public override void Initialize(float value, float maxValue)
    {
        base.Initialize(value, maxValue);        
        UpdateXpText(value, maxValue);
        SubscribeToEvents();
    }

     public void SetTrackedArmy(ArmyController armyController)
    {       
        if (_trackedArmy != null)
        {
            UnsubscribeFromEvents();
        }
        
        _trackedArmy = armyController;
        
        if (_trackedArmy != null)
        {
            // Yeni ordu olaylarına abone ol
            SubscribeToEvents();
            
            // İlk durumu ayarla
            UpdateXPDisplay(_trackedArmy.CurrentXP, _trackedArmy.XPToNextLevel);
            UpdateLevelDisplay(_trackedArmy.CurrentLevel);
        }        
    }
    
    private void SubscribeToEvents()
    {
        _trackedArmy.OnExperienceChanged += UpdateXPDisplay;
        _trackedArmy.OnLevelUp += UpdateLevelDisplay;
        _trackedArmy.OnArmyDestroyed += ResetDisplay;
    }
    
    private void UnsubscribeFromEvents()
    {
        _trackedArmy.OnExperienceChanged -= UpdateXPDisplay;
        _trackedArmy.OnLevelUp -= UpdateLevelDisplay;
        _trackedArmy.OnArmyDestroyed -= ResetDisplay;
    }   

    private void UpdateXPDisplay(float currentXP, float xpToNextLevel)
    {
        UpdateValue(currentXP, xpToNextLevel);
    }

    private void UpdateLevelDisplay(float level)
    {
        UpdateLevelText(level.ToString());
    }

    public override void UpdateValue(float value, float maxValue)
    {      
        _slider.value = value / maxValue;
        UpdateXpText(value, maxValue);

        if (_handleTween != null)
            _handleTween.Complete();       

        _handleTween = _handleSlider.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).OnComplete(() =>
        {
            _handleSlider.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
        });
    }

    private void UpdateLevelText(string level)
    {
        _levelText.text =  level;
    }

    private void UpdateXpText(float value, float maxValue)
    {
        _xpText.text = value + "/" + maxValue;
    }

    private void ResetDisplay()
    {      
        _levelText.text = "0";
        _xpText.text = "0/0";

        UnsubscribeFromEvents();
    }

}
