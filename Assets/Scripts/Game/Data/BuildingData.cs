public class BuildingData : EntityData
{   
    public float SpawnInterval { get; private set; }
    public float FirePower { get; private set; }

    public BuildingData(int level, float damage, float health, 
                        float armor, UnitRank rank, AgentUnitType unitType, 
                        Team team, float attackSpeed, float attackRange, 
                        float spawnInterval, float firePower)
        : base(level, damage, health, armor, rank, unitType, team, attackSpeed, attackRange)
    {        
        SpawnInterval = spawnInterval;
        FirePower = firePower;
    }

    public override void Initialize()
    {
        base.Initialize();        
    }
}