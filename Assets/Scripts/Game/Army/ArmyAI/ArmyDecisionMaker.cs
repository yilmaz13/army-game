using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum MarchPointType
{
    Defensive,
    Mid,
    Attack
}

public class ArmyDecisionMaker
{
    // Referanslar
    private ArmyController _army;
    private IUnitFormationController _formationController;
    private IArmyDataProvider _dataProvider;
    private Dictionary<MarchPointType, Vector3> _marchPoints;
    private Vector3 _castlePosition;
    
    // Durum takibi
    private bool _isAttacking = false;
    private Team _targetTeam;
    private Vector3 _targetPosition;
    
    // Zamanlayıcı
    private float _decisionTimer = 0f;
    private const float DECISION_INTERVAL = 6f; // 6 saniyede bir karar ver
    
    public ArmyDecisionMaker(ArmyController army, IUnitFormationController formationController, IArmyDataProvider dataProvider)
    {
        _army = army;
        _formationController = formationController;
        _dataProvider = dataProvider;
    }
    
    public void Initialize(Vector3 castlePosition, Dictionary<MarchPointType, Vector3> marchPoints)
    {
        _castlePosition = castlePosition;
        _marchPoints = marchPoints;
        
        // Başlangıçta savunma pozisyonu al
        MoveToDefensivePosition();
    }
    
    public void OnUpdate()
    {
        _decisionTimer += Time.deltaTime;
        
        if (_decisionTimer >= DECISION_INTERVAL)
        {
            _decisionTimer = 0f;
            MakeDecision();
        }
    }
    
    private void MakeDecision()
    {
        // En zayıf düşmanı bul
        ArmyController weakestEnemy = FindWeakestEnemy();
        
        if (weakestEnemy != null)
        {
            // Zayıf düşman varsa saldır
            AttackEnemy(weakestEnemy);
        }
        else
        {
            // Zayıf düşman yoksa savunmaya geç
            MoveToDefensivePosition();
        }
    }
    
    private ArmyController FindWeakestEnemy()
    {
        // Düşman ordularını bul
        List<ArmyController> enemies = _dataProvider.GetEnemyArmies();
        
        if (enemies == null || enemies.Count == 0)
            return null;
        
        // En zayıf düşmanı bul
        ArmyController weakest = null;
        float lowestPower = float.MaxValue;
        
        foreach (var enemy in enemies)
        {
            if (enemy == null || enemy.IsDefeated)
                continue;
                
            float power = enemy.GetArmyPower();
            
            if (power < lowestPower)
            {
                lowestPower = power;
                weakest = enemy;
            }
        }
        
        return weakest;
    }
    
    private void AttackEnemy(ArmyController enemy)
    {
        Debug.Log($"Army {_army.Team} is attacking {enemy.Team}!");
        _isAttacking = true;
        _targetTeam = enemy.Team;
        _targetPosition = enemy.transform.position;
        
        // Birliklerimizi düşmana doğru yönlendir
        _formationController.MoveUnitsWithTypeOrder(_targetPosition);
    }
    
    private void MoveToDefensivePosition()
    {
        Debug.Log($"Army {_army.Team} is taking defensive position.");
        _isAttacking = false;
        
        // Savunma pozisyonuna git
        if (_marchPoints.TryGetValue(MarchPointType.Defensive, out Vector3 defensePoint))
        {
            _formationController.MoveUnitsWithTypeOrder(defensePoint);
        }
        else
        {
            // Savunma pozisyonu tanımlanmamışsa kale önüne git
            Vector3 fallbackPosition = _castlePosition + new Vector3(0, 0, 3f);
            _formationController.MoveUnitsWithTypeOrder(fallbackPosition);
        }
    }
    
    public string GetCurrentStrategyName()
    {
        return _isAttacking ? "Attacking" : "Defensive";
    }
}

public enum StrategyType
{
    Defensive,
    Balanced,
    Aggressive
}