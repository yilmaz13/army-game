using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpawnSliderView : SliderView
{
    [SerializeField] private SpriteRenderer _SpawnBarMax;   
    [SerializeField] private TextMeshPro _spawnText;   

    public void SetTimer(float time)
    {
        SetSliderValue(0); 
        var spriteSize = _sliderInside.sprite.rect.size;   
        DOTween.To(() => _sliderInside.size.x, x => _sliderInside.size =
         new Vector2(x, _sliderInside.size.y), spriteSize.x * 0.01f, time).SetEase(Ease.Linear);
    }
}
