using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour, 
                              IAgentControllerListener                              

{
    [Header("Team Settings")]
    [SerializeField] private Team _team;
    [SerializeField] private bool _isPlayerControlled = false;
    
    [Header("Spawn Settings")]
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private int _maxSoldiers = 10;
    [SerializeField] private LayerMask _groundLayer = 1 << 6;
    
    [Header("Level System")]
    [SerializeField] private ArmyLevelData _levelData;
    [SerializeField] private int   _currentLevel = 1;
    [SerializeField] private float _currentXP = 0;
    [SerializeField] private float _xpToNextLevel;
    
    [Header("Resources & State")]
    [SerializeField] private GameResources _gameResources;
    [SerializeField] private bool _isDefeated = false;
    
    // PRIVATE FIELDS 
    private IArmyControllerListener _listener;
    private AgentFactory _agentFactory;
    private UnitManager _unitManager;
    private InputHandler _inputHandler;
    private CastleController _castleController;
    private ILevelUpStrategy _levelUpStrategy;
    private float _spawnTimer = 0f;

    public Action<float> OnLevelUp;
    public Action<float, float> OnExperienceChanged;

    public Action OnArmyDestroyed;   
    
    // PROPERTIES
    public Team Team           => _team;
    public bool IsDefeated     => _isDefeated;
    public int CurrentLevel    => _currentLevel;
    public float CurrentXP     => _currentXP;
    public float XPToNextLevel => _xpToNextLevel;

    private float _lastClickTime = 0f;
    private readonly float _clickThrottleTime = 0.2f; 
    
    private void Awake()
    {
        _agentFactory = new AgentFactory();
        _unitManager  = new UnitManager();      
    }  

    private void Update()
    {
        if (_isDefeated)
            return;

        HandleSpawning();
        
        
        _inputHandler?.ProcessInput(Camera.main);
    }

    private void HandleSpawning()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > _spawnInterval && _unitManager.UnitCount < _maxSoldiers)
        {
            _spawnTimer = 0f;
            SpawnSoldier();
            _castleController.ShowSpawnSlider(2);
            
            if (_team == Team.Red)
            {
                Vector3 spawnPosition = transform.position + new Vector3(0, 0, -1);
                _unitManager.MoveUnitsTo(spawnPosition);
            }
        }
    }   

    private void SpawnSoldier()
    {
        Vector3 spawnPosition = transform.position;
        AgentType selectedType = _agentFactory.GetRandomAvailableAgentType();
        
        if (selectedType == AgentType.None)
        {
            Debug.LogWarning("No available agent types to spawn");
            return;
        }

        AgentController agent = _agentFactory.CreateAgent(
            selectedType, 
            transform,
            spawnPosition, 
            _team, 
            this
        );
        
        if (agent != null)
        {
            _unitManager.AddAgent(agent);
            _listener.HandleAgentSpawned(agent);
        }
    }

    public void Initialize(IArmyControllerListener listener, Team team, GameResources gameResources, ArmyLevelData levelData)
    {
        _team = team;
        _listener = listener;
        _gameResources = gameResources;
        
        _levelData = levelData;
        _xpToNextLevel = _levelData.GetXPForLevel(_currentLevel);        
        
        _agentFactory.Initialize(gameResources.GetAgentStats());
        InitializeCastle();
        ConfigureTeam();    

        SetPlayerControlled();
        SetLevelUpStrategy();
        InitializeInputHandler();       
    }

    private void SetPlayerControlled()
    {
        if (Team == Team.Blue)
        {
            _isPlayerControlled = true;
        }
    }
    private void SetLevelUpStrategy()
    {
        if (_team == Team.Blue)
        {
            _levelUpStrategy = new PlayerLevelUpStrategy(this);
        }
        else
        {
            _levelUpStrategy = new BalancedLevelUpStrategy(this);
        }

         _levelUpStrategy.ApplyLevelUp();
    }
    private void InitializeCastle()
    {
        var castleStats = _gameResources.GetCastleStats();
        Vector3 spawnPosition = transform.position;
        
        GameObject castleObj = Instantiate(castleStats.prefab, spawnPosition, Quaternion.identity);



        var castleView = castleObj.GetComponent<CastleView>();
        var castle = castleObj.GetComponent<CastleController>();   
 
        LevelStats levelStats = castleStats.GetStatsForLevel(0);

        castle.Initialize(  levelStats,
                            castleView, 
                            this,
                            levelStats.rank,             
                            _team
                         );
        
        castle.SubscribeEvents(OnDefeatCastle);
        _unitManager.AddBuilding(castle);
        _castleController = castle;
        _listener.HandleAgentSpawned(castle);

        castleView.Initialize(Camera.main, 0);    
        
    }

    private void InitializeInputHandler(){

        _inputHandler = new InputHandler(_groundLayer, Camera.main);       
        _inputHandler.OnGroundClicked += HandleGroundClicked;        
    }

    //TODO 
    private void ConfigureTeam()
    {     
        if (_team == Team.Red)
        {
            _spawnInterval = 0.2f;
            _maxSoldiers = 1;
            _castleController.GetComponent<CastleView>().SetPoint(1);
        }
        else
        {
            _spawnInterval = 0.2f;
            _maxSoldiers = 1;
        }
    }

    public void Unload()
    {
   
    if (_castleController != null)
    {
        _castleController.UnsubscribeEvents(OnDefeatCastle);
    }
      
    if (_unitManager != null)
    {     
        _unitManager.RemoveAllUnit();  
       
        _unitManager.ClearAll();
    }    

    if (_agentFactory != null)
    {
        _agentFactory.Clear();
        _agentFactory = null;
    }
    
    if (_inputHandler != null)
    {
        _inputHandler.Dispose();
        _inputHandler = null;
    }    

    _castleController = null;
    _listener = null;
    _gameResources = null;

    _spawnTimer = 0f;    

    _isDefeated = false;   

    OnArmyDestroyed?.Invoke(); 
   
    }
 
    public IDamageable NearestEnemy(Vector3 position, float range, Team currentTeam)
    {
        return _listener.NearestEnemy(position, range, currentTeam);
    }

    private void HandleGroundClicked(Vector3 position)
    {
        if (_team == Team.Blue)
        {
            float currentTime = Time.time;
            if (currentTime - _lastClickTime >= _clickThrottleTime)
            {
                _lastClickTime = currentTime;
                _unitManager.MoveUnitsTo(position);
            }
        }
    }

    public void HandleAgentDied(IDamageable entity, IDamageable killer)
    {       
        if (entity != null && killer.GetTeam() != Team)
        {
            if (entity is AgentController agent)
            { 
                _unitManager.RemoveAgent(agent);
            }
            else if (entity is BuildingController building)
            {     
                _unitManager.RemoveBuilding(building);
            }     
        }        
        
        _listener.HandleAgentDied(entity);
    }    

    public void HandleAgentKilled(IDamageable victim)
    {
        UnitRank rank = UnitRank.Regular;
        if (victim is AgentController agent) 
            rank = agent.Rank;
        else if (victim is BuildingController building) 
            rank = building.Rank;

        AddExperience(_levelData.GetXPForKill(rank));
    }

    public void OnDefeatCastle()
    {
        _isDefeated = true;
        _listener.OnDefeatCastle(_team);
        OnArmyDestroyed?.Invoke();
    }
    public void AddAvailableAgentType(AgentType agentType)
    {
        if  (_agentFactory.IsAgentTypeAvailable(agentType))
             _agentFactory.UpgradeAgentLevel(agentType);
        else
        _agentFactory.AddAvailableAgentType(agentType);
       
    }

    public List<AgentController> GetUnits()
    {
        return _unitManager.GetUnits();
    }

    public void AddExperience(int xpAmount)
    {
        _currentXP += xpAmount;        
        
        OnExperienceChanged?.Invoke(_currentXP, _xpToNextLevel);

        if (_currentXP >= _xpToNextLevel)
        {
            LevelUp();
        }
    }    

    [ContextMenu("Level Up")]
    private void LevelUp()
    {
        _currentXP -= _xpToNextLevel;
        _currentLevel++; 
        
        _xpToNextLevel = _levelData.GetXPForLevel(_currentLevel);       

        OnLevelUp?.Invoke(_currentLevel);
        OnExperienceChanged?.Invoke(_currentXP, _xpToNextLevel);        
      
        _levelUpStrategy.ApplyLevelUp();       
        
        _castleController.ShowLevelUpNotification(_currentLevel);             
       
        if (_currentXP >= _xpToNextLevel)
        {
            LevelUp();
        }
    }   

    public void UpgradeAgentType(AgentType type)
    {
        _agentFactory.UpgradeAgentLevel(type);
        
        string agentName = type.ToString();
        if (_castleController != null)
        {
            string description = "Units upgraded!";
            if (_levelData != null)
            {
                description = _levelData.GetUpgradeDataForAgentType(type).upgradeDescription;
            }
            
            _castleController.ShowUpgradeNotification(agentName, description);
        }
    }    

    public AgentType[] GetUpgradeOptionsForCurrentLevel()
    {
        return _levelData.GetUpgradeOptionsForLevel(_currentLevel);        
    }

    private void OnDestroy()
    {       
        Unload();    
        OnLevelUp = null;
        OnExperienceChanged = null;
        OnArmyDestroyed = null;
    }
}