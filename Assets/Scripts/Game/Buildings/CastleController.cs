using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CastleController : BuildingController, IDamageable
{  
    CastleView _castleView;   
    public Action OnCastleDestroyed;
    //TODO: bağımsız notification bir yapı oluşturulacak
    [SerializeField] private GameObject _notificationPanel;
    [SerializeField] private TextMeshPro _notificationText;
    [SerializeField] private float _notificationDuration = 2f;
    protected override void TakeDamage(float damage, IDamageable shooter, float armorPenetration = 100)
    {
        base.TakeDamage(damage, shooter, armorPenetration);        
        DamageView(); 
    }   

    private void DamageView(){
        _castleView.DamageCastle();
    }

    protected override void OnHealthHalf()
    {
        _castleView.RuinedLowCastle();
    }    

    protected override void OnHealthThreeQuarter()
    {
        _castleView.RuinedHighCastle();
    }

    protected override void OnHealthTenPercent()
    {
        _castleView.DamageCastle();
    }

    protected override void OnDead(IDamageable killer)
    {
        base.OnDead(killer);
        _castleView.CastleExplosion();
        _castleView.CreatFractures(Level);
        OnCastleDestroyed?.Invoke();
    }

    public void Initialize(LevelStats stats,
                           CastleView buildingView, 
                           IAgentControllerListener listener,
                           UnitRank rank,
                           AgentUnitType unitType,
                           Team team)
                          
    {    
        _castleView = buildingView;

        base.Initialize(stats, 
                        buildingView, 
                        listener,                        
                        rank, 
                        unitType,
                        team);
        
    }

    public void SubscribeEvents (Action OnCastleDestroyed)
    {
        this.OnCastleDestroyed += OnCastleDestroyed;
    }

    public void UnsubscribeEvents (Action OnCastleDestroyed)
    {
        this.OnCastleDestroyed -= OnCastleDestroyed;
    }    
 
    public void ShowSpawnSlider(int time)
    {
        _castleView.SetSpawnSliderTimer(time);
    }

    public void SetSpawnSliderValue(float value)
    {
        _castleView.SetSpawnSliderValue(value);
    }

    public Vector3 GetCastleViewRotation()
    {
        return _castleView.GetCastleRotation();
    }

    public void ShowLevelUpNotification(int newLevel)
    {
        _notificationText.gameObject.SetActive(true);

        _notificationText.text = $"Castle Level Up! Your army is now level {newLevel}";

        _notificationText.transform.DOMoveY(_notificationText.transform.position.y + 1 , _notificationDuration).onComplete += () =>
        {
           _notificationText.gameObject.SetActive(false);
        };
    }

    public void ShowUpgradeNotification(string unitName, string description = "")
    {
        _notificationText.gameObject.SetActive(false);

        _notificationText.text = $"{unitName} units upgraded! {description}";

        _notificationText.transform.DOMoveY(_notificationText.transform.position.y + 1 , _notificationDuration).onComplete += () =>
        {
           _notificationText.gameObject.SetActive(false);
        };
    }

    public Transform GetMarchPoint(MarchPointType pointType)
    {
        if (_castleView != null)
        {
            switch (pointType)
            {
                case MarchPointType.Defensive:
                    return _castleView.GetMarchPointDefensive();
                case MarchPointType.Attack:
                    return _castleView.GetMarchPoint();
                case MarchPointType.Mid:
                    return _castleView.GetMarchPointMid();
            }           
        }        
   
        return transform;
    }

    public Transform GetUnitSpawnerPoints()
    {
        if (_castleView != null)
        {
            return _castleView.GetUnitSpawnerPoints();
        }        

        return transform;
    }
}