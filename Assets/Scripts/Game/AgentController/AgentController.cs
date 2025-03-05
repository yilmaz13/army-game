using DG.Tweening;
using UnityEngine;

public class AgentController : MonoBehaviour, IDamageable, IPoolable
{
    // Private Values
    protected AgentData _agentData;
    protected IAgentControllerListener _listener;
    private AgentView _agentView;
    private IDamageable _targetEnemy;
    private AgentState _currentState;        
    
    protected bool _isActive;
    private float _attackCooldown;
    private float _lastAttackTime;
    
    // Properties
    public int Level => _agentData.Level;
    public float Damage => _agentData.Damage;
    public float Speed => _agentData.Speed;
    public float Health => _agentData.Health;
    public float Armor => _agentData.Armor;
    public UnitRank Rank => _agentData.Rank;
    public Team GetTeam => _agentData.Team;

    public AgentUnitType UnitType => _agentData.UnitType;
    public bool IsActive => _isActive;
    public Vector3 IdleRotation => _agentData.IdleRotation;
    public Vector3 LastPosition { get; set; }
    public HealthController HealthController { get; private set; }
    public ArmorController ArmorController { get; private set; }
    public AgentData Data => _agentData;

    private Tween decideActionTween;

    [SerializeField] private float _idleCheckDelay = 0.5f; 
    [SerializeField] private float _combatCheckDelay = 0.2f;
    [SerializeField] private float _chaseCheckDelay = 0.4f; 
    
    // Initialize Methods
    public void Initialize(
        LevelStats stats,
        AgentView agentView, 
        IAgentControllerListener listener, 
        UnitRank rank,
        AgentUnitType unitType,
        Team team,
        Vector3 idleRotation)
    {
        _listener = listener;
        _agentView = agentView;        
        _attackCooldown = stats.attackSpeed;
        
        InitializeAgentData(stats, rank, unitType, team, idleRotation);
        InitializeControllers();
        InitializeVisuals(team);
        
        _isActive = true;
        _lastAttackTime = -_attackCooldown;
        _currentState = AgentState.Idle;
    
        DOVirtual.DelayedCall(0.2f, StartAgentBehavior); 
    }
    
    private void InitializeAgentData(LevelStats stats, UnitRank rank, AgentUnitType unitType, Team team, Vector3 idleRotation)
    {
        _agentData = new AgentData(stats.level, stats.damage, stats.attackSpeed, stats.health, 
                                   stats.armor, stats.speed, stats.attackRange, stats.chaseRange,
                                   rank, unitType, team, idleRotation);                                 
        _agentData.Initialize();
    }
    
    private void InitializeControllers()
    {
        HealthController = new HealthController();
        ArmorController = new ArmorController();
        
        HealthController.Initialize(Health);
        ArmorController.Initialize(Armor);
        SubscribeHealthEvents();
    }
    
    private void InitializeVisuals(Team team)
    {
        _agentView.InitializeHealthBar(HealthController.Value, HealthController.MaxValue, team);
        _agentView.SetMaterial(team);
        _agentView.SetSpeed(Speed);
        _agentView.ResetNavMeshAgent();
        _agentView.SetHealthBarRotation(IdleRotation);
        InvokeRepeating("SetHealthBarRotation", 0.1f, 0.05f);
    }    
    
    private void SetHealthBarRotation()
    {
        if  (_agentView != null)
            _agentView.SetHealthBarRotation(IdleRotation);
    }

    private void StartAgentBehavior()
    {       
        UpdateEnemyTarget();
    } 

    private void UpdateEnemyTarget()
    {
        if (!_isActive) return;           

        if (!HasActiveEnemy())
        {
            _targetEnemy = FindNearestEnemy();

            if (!HasActiveEnemy())
            {
                if (ShouldBecomeIdle())
                {
                    BecomeIdle();
                }
                else
                    decideActionTween =  DOVirtual.DelayedCall(_idleCheckDelay, UpdateEnemyTarget);
            }
            else    
            {
                DecideNextAction();
            }
        }
        else    
        {
                DecideNextAction();
        }
    }
    
    private IDamageable FindNearestEnemy()
    {
        return _listener.NearestEnemy(transform.position, _agentData.ChaseRange, GetTeam);
    }    

    [ContextMenu("FindNearestEnemyTest")]
    private void FindNearestEnemyTest()
    {
        var enemy = FindNearestEnemy();       
    }
    
    private void DecideNextAction()
    {
        if (HasActiveEnemy())
        {
            HandleCombatActions();
        }
        else if (ShouldBecomeIdle())
        {
            BecomeIdle();
        }
    }
    
    private bool HasActiveEnemy()
    {
        return _targetEnemy != null && _targetEnemy.IsActive();
    }
    
    private bool ShouldBecomeIdle()
    {
        return _currentState == AgentState.Moving && HasReachedDestination();
    }
    
    private void HandleCombatActions()
    {
        Vector3 enemyPosition = _targetEnemy.GetPosition();
        
        if (IsWithinAttackRange(enemyPosition))
        {
            AttackTarget();
        }
        else
        {
            ChaseTarget();
        }
    }
    
    private bool IsWithinAttackRange(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= _agentData.AttackRange;
    }    

    private void AttackTarget()
    {
        StopMovement();
        _agentView.LookAtPosition(_targetEnemy.GetPosition());
        
        if (CanAttack())
        {
            PerformAttack();  
            decideActionTween = DOVirtual.DelayedCall(_attackCooldown, StartAgentBehavior);            
        }
        else
        {
            decideActionTween = DOVirtual.DelayedCall(_combatCheckDelay, StartAgentBehavior);
        }
    }
    
    private bool CanAttack()
    {
        return Time.time >= _lastAttackTime + _attackCooldown;
    }
    
    private void PerformAttack()
    {
        _targetEnemy.ApplyDamage(Damage, this);
        _agentView.Attack();
        _lastAttackTime = Time.time;
        _currentState = AgentState.Attacking;
    }
    
    private void ChaseTarget()
    {
        StopMovement();
        _currentState = AgentState.Chasing;
        _agentView.MoveTo(_targetEnemy.GetPosition());

        decideActionTween = DOVirtual.DelayedCall(_chaseCheckDelay, StartAgentBehavior);
    }
    
    private void BecomeIdle()
    {
        _agentView.Idle();
        _currentState = AgentState.Idle;
        transform.DOLocalRotate(IdleRotation, 0.15f).SetDelay(Random.Range(0.05f, 0.1f));

        decideActionTween = DOVirtual.DelayedCall(_idleCheckDelay, StartAgentBehavior);
    }    

    public void MoveTo(Vector3 destination)
    {
        if(Vector3.Distance(destination, transform.position) < 0.1f) 
        return;
        _agentView.MoveTo(destination);
        _currentState = AgentState.Moving;
      //  transform.DOLocalRotate(IdleRotation, 0.1f);
    }
    
    public void StopMovement()
    {
        _agentView.StopMovement();
    }
    
    private bool HasReachedDestination()
    {
        var agent = _agentView.NavMeshAgent;
        
        return !agent.pathPending && 
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }    

    public void ApplyDamage(float damage, IDamageable shooter, float armorPenetration = 100)
    {
        TakeDamage(damage, shooter, armorPenetration);
        UpdateHealthVisuals();
    }
    
    private void TakeDamage(float damage, IDamageable shooter, float armorPenetration)
    {
        float remainingDamage = ArmorController.AbsorbDamage(damage, armorPenetration);
        HealthController.TakeDamage(remainingDamage, shooter);
    }
    
    private void UpdateHealthVisuals()
    {
        _agentView.UpdateHealthBar(HealthController.Value, HealthController.MaxValue);
    }    

    private void SubscribeHealthEvents()
    {
        HealthController.OnDead += OnDead;
    }
    
    private void UnsubscribeHealthEvents()
    {
        if (HealthController != null)
        {
            HealthController.OnDead -= OnDead;           
        }
    }
    
    protected virtual void OnDead(IDamageable killer)
    {
        UnsubscribeHealthEvents();
        CancelInvoke("SetHealthBarRotation");
        _isActive = false;
        _agentView.Die();
        _listener.HandleAgentDied(this, killer);
    
         if (killer is AgentController killerAgent)
        {      
            killerAgent.NotifyKill(this);
        }
    }    

    public void NotifyKill(IDamageable victim)
    {       
        _listener.HandleAgentKilled(victim);
    }

    public virtual void Upgrade()
    {
        
    }

    public bool IsUnitAttacking()
    {       
        return _currentState == AgentState.Attacking;
    }
    
    
    // IDamageable Interface
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    Team IDamageable.GetTeam()
    {
        return GetTeam;
    }
    
    bool IDamageable.IsActive()
    {
        return IsActive;
    }   

    // IPoolable Interface

    public void OnReturnPool()
    {
        //UnsubscribeHealthEvents();        
       
        _isActive = false;
        _targetEnemy = null;
        _currentState = AgentState.Idle;       
        
        if (_agentView != null)
        {
            _agentView.ResetVisuals();
        }
    }

    public void OnPoolSpawn()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true); 
        
        if (_agentView != null)
        {
            _agentView.ResetNavMeshAgent();
            _agentView.ResetVisuals();
        }      

         if (HealthController != null) HealthController.Reset();
         if (ArmorController != null) ArmorController.Reset();

     
       
        _isActive = true;
        _lastAttackTime = -_attackCooldown;
        _currentState = AgentState.Idle;     
       
    }
}