public class AgentData: EntityData
{
    // PROTECTED FIELDS   
    private float _speed;
    private float _chaseRange;   

    // PUBLIC PROPERTIES 
    public float Speed => _speed;
    public float ChaseRange => _chaseRange;   

    // CONSTRUCTOR
    public AgentData(int level, float damage, float speed, 
                     float health, float armor, float attackSpeed,
                     float attackRange, float chaseRange, UnitRank rank, Team team)
        : base(level, damage, health, armor, rank, team, attackSpeed, attackRange)
    {       
        _speed = speed;       
        _chaseRange = chaseRange;
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