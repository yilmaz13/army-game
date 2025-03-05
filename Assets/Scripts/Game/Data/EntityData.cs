public class EntityData
{
    // PROTECTED FIELDS   
    protected int _level;
    protected float _damage;
    protected float _attackSpeed;
    protected float _health;
    protected float _armor;
    protected float _attackRange;
    protected UnitRank _rank; 
    protected AgentUnitType _unitType;
    protected Team _team;   

    // PUBLIC PROPERTIES
    public int Level => _level;
    public float Damage => _damage;
    public float AttackSpeed => _attackSpeed;
    public float Health => _health;
    public float Armor => _armor;
    public float AttackRange => _attackRange;
    public UnitRank Rank => _rank; 
    public Team Team => _team;   

    public AgentUnitType UnitType => _unitType;

    public EntityData(int level, 
                      float damage,
                      float health, 
                      float armor, 
                      UnitRank rank,
                      AgentUnitType agentUnitType,
                      Team team, 
                      float attackSpeed,
                      float attackRange)
    {
        _level = level;
        _damage = damage;
        _health = health;
        _armor = armor;
        _rank = rank;
        _team = team;
        _attackSpeed = attackSpeed;
        _attackRange = attackRange;    
        _unitType = agentUnitType;     
    }

    public virtual void Initialize()
    {
        
    }
}