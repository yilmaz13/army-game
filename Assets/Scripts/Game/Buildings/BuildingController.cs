using UnityEngine;

public class BuildingController : MonoBehaviour, IDamageable, IPoolable
{
    // protected Values
    protected BuildingData _buildingData;
    protected IAgentControllerListener _listener;
    protected BuildingView _buildingView;
    
    // Private Values
    private float _attackCooldown;
    private float _lastAttackTime;
    private bool _isActive;
    
     // Public Values
    public int Level => _buildingData.Level;
    public float Damage => _buildingData.Damage;   
    public float Health => _buildingData.Health;
    public float Armor => _buildingData.Armor;
    public UnitRank Rank => _buildingData.Rank;
    public Team GetTeam => _buildingData.Team;
    public BuildingData Data => _buildingData;
    public HealthController HealthController { get; private set; }
    public ArmorController ArmorController { get; private set; }
    
    // Initialize Methods
    public virtual void Initialize(
                                    LevelStats stats,
                                    BuildingView buildingView, 
                                    IAgentControllerListener listener,
                                    UnitRank rank,
                                    AgentUnitType unitType,
                                    Team team)
    {
        _listener = listener;
        _buildingView = buildingView;
        _attackCooldown = stats.attackSpeed;     

        InitializeBuildingData(stats, rank, unitType, team);
        InitializeControllers();
        InitializeVisuals(team);
        
        _isActive = true;
        _lastAttackTime = -_attackCooldown;
    }
    
    private void InitializeBuildingData(LevelStats stats, UnitRank rank, 
                                        AgentUnitType unitType, Team team)
    {       
         _buildingData = new BuildingData(stats.level, stats.damage, stats.health, 
                                        stats.armor, rank, unitType, team, stats.attackRange, 
                                        stats.attackSpeed,0,0);

        _buildingData.Initialize();
    }
    
    private void InitializeControllers()
    {
        InitializeHealthAndArmorController(Health, Armor);
        SubscribeHealthEvents();
    }
    
    private void InitializeVisuals(Team team)
    {
        InitializeView();
        _buildingView.SetMaterial(team);
    }
    
    private void InitializeHealthAndArmorController(float maxHealth, float maxArmor)
    {
        HealthController = new HealthController();
        ArmorController = new ArmorController();

        HealthController.Initialize(maxHealth);
        ArmorController.Initialize(maxArmor);
    }

    private void InitializeView()
    {
        _buildingView.InitializeHealthBar(HealthController.Value, HealthController.MaxValue, GetTeam);
      
    }    
   
    public virtual void ApplyDamage(float damage, IDamageable shooter, float armorPenetration = 100)
    {
        TakeDamage(damage, shooter, armorPenetration);
        UpdateViewBars();
    }
    
    protected virtual void TakeDamage(float damage, IDamageable shooter, float armorPenetration)
    {
        float remainingDamage = ArmorController.AbsorbDamage(damage, armorPenetration);
        HealthController.TakeDamage(remainingDamage, shooter);
    }
    
    private void UpdateViewBars()
    {
        _buildingView.UpdateHealthBar(HealthController.Value, HealthController.MaxValue);
    }    

    private void SubscribeHealthEvents()
    {
        HealthController.OnDead += OnDead;
        HealthController.OnHealthHalf += OnHealthHalf;
        HealthController.OnHealthThreeQuarter += OnHealthThreeQuarter;
        HealthController.OnHealthTenPercent += OnHealthTenPercent;
    }

    private void UnsubscribeHealthEvents()
    {
        HealthController.OnDead -= OnDead;
        HealthController.OnHealthHalf -= OnHealthHalf;
        HealthController.OnHealthThreeQuarter -= OnHealthThreeQuarter;
        HealthController.OnHealthTenPercent -= OnHealthTenPercent;
    }    
   
    protected virtual void OnDead(IDamageable killer)
    {
        UnsubscribeHealthEvents();
        _isActive = false;
        _buildingView.Die();        

        if (_listener != null)
        {
            _listener.HandleAgentDied(this, killer);
        }
    }
    
    protected virtual void OnHealthHalf()
    {
        _buildingView.OpenHalfHealthVFX();
    }

    protected virtual void OnHealthThreeQuarter()
    {
        _buildingView.OpenThreeQuarterHealthVFX();
    }

    protected virtual void OnHealthTenPercent()
    {
        _buildingView.OpenTenPercentHealthVFX();
    }    

    protected bool CanAttack()
    {
        return Time.time >= _lastAttackTime + _attackCooldown;
    }
    
    protected void ResetAttackTimer()
    {
        _lastAttackTime = Time.time;
    }
    
    // IDamageable Interface
    Vector3 IDamageable.GetPosition() => transform.position;
    Team IDamageable.GetTeam() => GetTeam;
    bool IDamageable.IsActive() => _isActive;   

    public void OnReturnPool()
    {
       UnsubscribeHealthEvents();
        _isActive = false;
        _lastAttackTime = 0;
      
    }

    public void OnPoolSpawn()
    {
        
    }
}