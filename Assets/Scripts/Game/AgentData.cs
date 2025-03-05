using UnityEngine;

public class AgentData: EntityData
{
    // PROTECTED FIELDS   
    private float _speed;
    private float _chaseRange;   

    private Vector3 _idleRotation;

    // PUBLIC PROPERTIES 
    public float Speed => _speed;
    public float ChaseRange => _chaseRange;   
    public Vector3 IdleRotation => _idleRotation;
    // CONSTRUCTOR
    public AgentData(int level, float damage, float speed, 
                     float health, float armor, float attackSpeed,
                     float attackRange, float chaseRange, UnitRank rank, AgentUnitType unitType, Team team, Vector3 idleRotation)
        : base(level, damage, health, armor, rank, unitType, team, attackSpeed, attackRange)
    {       
        _speed = speed;       
        _chaseRange = chaseRange;
        _idleRotation = idleRotation;
    } 

    // PUBLIC METHODS

    public override void Initialize()
    {        
    }

    public void Upgrade(LevelStats stats)
    {
        _level++;
        /*
        _damage *= stats.damageMultiplier;
        _speed *= stats.speedMultiplier;
        _health *= stats.healthMultiplier;
        _armor *= stats.armorMultiplier;
        */
    }
}