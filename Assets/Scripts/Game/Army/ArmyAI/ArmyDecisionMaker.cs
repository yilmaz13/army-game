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
    private ArmyController _army;
    private IUnitFormationController _formationController;
    private IArmyDataProvider _dataProvider;
    private Dictionary<MarchPointType, Vector3> _marchPoints;
    private Vector3 _castlePosition;
   
    private bool _isAttacking = false;
    private Team _targetTeam;
    private Vector3 _targetPosition;    
   
    private float _decisionTimer = 0f;
    private const float DECISION_INTERVAL = 10f;
    
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
        ArmyController weakestEnemy = FindWeakestEnemy();
        
        float power = GetArmyPower();

        if (weakestEnemy != null)
        {
            if  (power > weakestEnemy.GetArmyPower())
            {              
                AttackEnemy(weakestEnemy);
            }
        }
        else
        {           
            MoveToDefensivePosition();
        }
    }
    
    private ArmyController FindWeakestEnemy()
    {      
        List<ArmyController> enemies = _dataProvider.GetEnemyArmies();
        
        if (enemies == null || enemies.Count == 0)
            return null;        
    
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
    
    private float GetArmyPower()
    {        
       return  _army.GetArmyPower();        
    }
    private void AttackEnemy(ArmyController enemy)
    {
        Debug.Log($"Army {_army.Team} is attacking {enemy.Team}!");
        _isAttacking = true;
        _targetTeam = enemy.Team;
        _targetPosition = enemy.transform.position;
              
        _formationController.MoveUnitsWithTypeOrder(_targetPosition);
    }
    
    private void MoveToDefensivePosition()
    {
        Debug.Log($"Army {_army.Team} is taking defensive position.");
        _isAttacking = false;        
        
        if (_marchPoints.TryGetValue(MarchPointType.Defensive, out Vector3 defensePoint))
        {
            _formationController.MoveUnitsWithTypeOrder(defensePoint);
        }
        else
        {           
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