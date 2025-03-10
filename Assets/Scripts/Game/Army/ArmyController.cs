using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour, 
                              IAgentControllerListener,
                              IUnitFormationController,
                              IArmyDataProvider                         

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

    [Header("AI Settings")]
    [SerializeField] private bool _useAI = false;
    [SerializeField] private StrategyType _initialStrategy = StrategyType.Balanced;
    
    // PRIVATE FIELDS 
    private IArmyControllerListener _listener;
    private IArmyInformation _armyInformation;
    private AgentFactory _agentFactory;
    private UnitManager _unitManager;
    private InputHandler _inputHandler;
    private CastleController _castleController;
    private ILevelUpStrategy _levelUpStrategy;
    private float _spawnTimer = 0f;
    private ArmyDecisionMaker _decisionMaker;

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
    private Vector3 _lastClickPosition;
    private bool _hasLastClickPosition = false;
    
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
        
        if (!_isPlayerControlled && _decisionMaker != null)
        {
            _decisionMaker.OnUpdate();
        }
        else 
        {
            _inputHandler?.ProcessInput(Camera.main);
        }
    }

    private void HandleSpawning()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > _spawnInterval && _unitManager.UnitCount < _maxSoldiers)
        {
            _spawnTimer = 0f;
            SpawnSoldier();
            _castleController.ShowSpawnSlider(2);
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
            this,
            GetCastleRotation()
        );
        
        if (agent != null)
        {
            // Önce birimi UnitManager'a ekle
            _unitManager.AddAgent(agent);
            _listener.HandleAgentSpawned(agent);
            
            // Formasyon ile hareket ettir
            if (_isPlayerControlled && _hasLastClickPosition)
            {
                // Oyuncu kontrolündeyse, tüm birimleri formasyon halinde yeni pozisyona gönder
                // Bunu UnitManager.MoveUnitsTo ile yapmak yerine, yeni bir metod kullanacağız
                _unitManager.FormationAddUnit(agent, _lastClickPosition);
            }
            else if (!_isPlayerControlled)
            {
                // AI kontrolündeyse ve defensive march point varsa
                if (_castleController != null)
                {
                    // Defensive pozisyona birimi formasyon içinde ekle
                    Transform defensePoint = _castleController.GetMarchPoint(MarchPointType.Defensive);
                    if (defensePoint != null)
                    {
                        _unitManager.FormationAddUnit(agent, defensePoint.position);
                    }
                }
            }
        }
    }

    public void Initialize(IArmyControllerListener listener, Team team, GameResources gameResources, ArmyLevelData levelData, IArmyInformation armyInformation)
    {
        _team = team;
        _listener = listener;
        _gameResources = gameResources;
        _armyInformation = armyInformation;
        
        _levelData = levelData;
        _xpToNextLevel = _levelData.GetXPForLevel(_currentLevel);        
        
        _agentFactory.Initialize(gameResources.GetAgentStats());
        InitializeCastle();
        ConfigureTeam();    

        SetPlayerControlled();
        SetLevelUpStrategy();
        InitializeInputHandler();
        
        if (!_isPlayerControlled) 
        {
            InitializeAI();
        }
    }

    private void InitializeAI()
    {
        if (_isPlayerControlled) return;    
        
        var marchPoints = new Dictionary<MarchPointType, Vector3>();
        
        if (_castleController != null)
        {          
            marchPoints[MarchPointType.Defensive] = _castleController.GetMarchPoint(MarchPointType.Defensive).position;
            marchPoints[MarchPointType.Mid] = _castleController.GetMarchPoint(MarchPointType.Mid).position;
            marchPoints[MarchPointType.Attack] = _castleController.GetMarchPoint(MarchPointType.Attack).position;
        }        
      
        _decisionMaker = new ArmyDecisionMaker(this, this, this);
        _decisionMaker.Initialize(transform.position, marchPoints);
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
                            castleStats.unitType,           
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
            _spawnInterval = 5f;
            _maxSoldiers = 10;
            _castleController.GetComponent<CastleView>().SetPoint(1);
        }
        else
        {
            _spawnInterval = 2f;
            _maxSoldiers = 15;
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
                _lastClickPosition = position;
                _hasLastClickPosition = true;
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

    public Vector3 GetCastleRotation()
    {
        return _castleController.GetCastleViewRotation();        
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

    public CastleController GetCastleController()
    {
        return _castleController;
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

    private void OnDrawGizmos()
    {
        if (_unitManager != null && _unitManager.IsFormationVisible)
        {         
            Gizmos.color = _team == Team.Blue ? new Color(0, 0.5f, 1f, 0.8f) : new Color(1f, 0.3f, 0.3f, 0.8f);
            Gizmos.DrawSphere(_unitManager.FormationTarget, 0.5f);            
         
            Gizmos.color = _team == Team.Blue ? new Color(0, 0.5f, 1f, 0.4f) : new Color(1f, 0.3f, 0.3f, 0.4f);
            
            var positions = _unitManager.FormationPositions;
            foreach (var pos in positions)
            {              
                Gizmos.DrawSphere(pos, 0.3f);
            }
                      
            if (positions.Count > 1)
            {
                Gizmos.color = _team == Team.Blue ? new Color(0, 0.3f, 0.8f, 0.6f) : new Color(0.8f, 0.2f, 0.2f, 0.6f);                
              
                for (int i = 0; i < positions.Count - 1; i++)
                {                    
                    bool isNewRow = (i % 9 == 8);
                    
                    if (!isNewRow)
                    {
                        Gizmos.DrawLine(positions[i], positions[i + 1]);
                    }
                }                
              
                if (positions.Count > 0)
                {
                    DrawArrow(_unitManager.FormationTarget, positions[0], 0.4f);
                }
            }
        }
    }

    private void DrawArrow(Vector3 from, Vector3 to, float arrowHeadSize)
    {
        Gizmos.DrawLine(from, to);
        
        Vector3 direction = (to - from).normalized;
        Vector3 right = Quaternion.Euler(0, 30, 0) * -direction * arrowHeadSize;
        Vector3 left = Quaternion.Euler(0, -30, 0) * -direction * arrowHeadSize;
        
        Gizmos.DrawLine(to, to + right);
        Gizmos.DrawLine(to, to + left);
    }

    public Vector3 GetIdleRotation()
    {
       return GetCastleRotation();
    }

    public void MoveUnitsToDefensiveFormation(Vector3 position)
    {
        _unitManager.MoveUnitsWithTypeOrder(position);
    }
    
    
    public void MoveUnitsWithTypeOrder(Vector3 position)
    {
        _unitManager.MoveUnitsWithTypeOrder(position);
    }

    public void MoveSpecificUnitsTo(List<AgentController> units, Vector3 position)
    {
        _unitManager.MoveSpecificUnitsTo(units, position);
    }
    
    public float GetArmyPower()
    {
        float power = 0;
        var units = GetUnits();
        foreach (var unit in units)
        {
            if (unit != null)
            {
                power += unit.Level * (unit.HealthController.Value / unit.HealthController.MaxValue);
            }
        }
        return power + (CurrentLevel * 10);
    }
    
    private void OnGUI()
    {
        /*
        if (_useAI && !_isPlayerControlled && _decisionMaker != null && !_isDefeated)
        {
            string strategyText = _decisionMaker.GetCurrentStrategyName();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 5);
            GUI.color = _team == Team.Blue ? Color.blue : Color.red;
            GUI.Label(new Rect(screenPos.x - 50, Screen.height - screenPos.y - 20, 100, 20), 
                $"[{strategyText}]");
        }
        */
    }

    public void MoveSpecificUnitsTo(List<int> unitIDs, Vector3 targetPosition)
    {
        var units = new List<AgentController>();
        foreach (var unit in GetUnits())
        {
            if (unitIDs.Contains(unit.GetInstanceID()))
            {
                units.Add(unit);
            }
        }
        
        _unitManager?.MoveSpecificUnitsTo(units, targetPosition); 
    }

    public int GetUnitCount()
    {
        return _unitManager?.UnitCount ?? 0;
    }
    
    public float GetEnemyPower()
    {
        ArmyController enemy = _armyInformation.GetArmyByTeam(Team == Team.Blue ? Team.Red : Team.Blue);
        return enemy?.GetArmyPower() ?? 0;
    }
    
    public Vector3 GetEnemyPosition()
    {
        ArmyController enemy = _armyInformation.GetArmyByTeam(Team == Team.Blue ? Team.Red : Team.Blue);
        return enemy?.transform.position ?? Vector3.zero;
    }
    
    public bool IsEnemyDefeated()
    {
        ArmyController enemy = _armyInformation.GetArmyByTeam(Team == Team.Blue ? Team.Red : Team.Blue);
        return enemy == null || enemy.IsDefeated;
    }

    public List<int> GetUnitIDs()
    {
        var units = GetUnits();
        var ids = new List<int>(units.Count);
        
        foreach (var unit in units)
        {
            if (unit != null)
            {
                ids.Add(unit.GetInstanceID());
            }
        }
        
        return ids;
    }

    public List<int> GetTankUnitIDs()
    {
        var units = GetUnits();
        var ids = new List<int>();
        
        foreach (var unit in units)
        {
            if (unit != null && unit.UnitType == AgentUnitType.Tank)
            {
                ids.Add(unit.GetInstanceID());
            }
        }
        
        return ids;
    }

    public List<int> GetRangedUnitIDs()
    {
        var units = GetUnits();
        var ids = new List<int>();
        
        foreach (var unit in units)
        {
            if (unit != null && unit.UnitType == AgentUnitType.Ranged)
            {
                ids.Add(unit.GetInstanceID());
            }
        }
        
        return ids;
    }

   public List<ArmyController> GetEnemyArmies()
    {
        if (_armyInformation == null) return new List<ArmyController>();
    
        return _armyInformation.GetArmiesByTeam(_team == Team.Blue ? Team.Red : Team.Blue);
    }
}