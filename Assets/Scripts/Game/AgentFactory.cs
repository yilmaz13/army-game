using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.PoolSystem;

public class AgentFactory
{
    private Dictionary<AgentType, int> _agentLevels = new Dictionary<AgentType, int>();
    private List<AgentType> _availableAgentTypes = new List<AgentType>();
    private AgentStats[] _agentStatsList;
    
    private Dictionary<AgentType, string> _prefabNameCache = new Dictionary<AgentType, string>();
    private Dictionary<AgentType, AgentStats> _agentStatsCache = new Dictionary<AgentType, AgentStats>();

    public void Initialize(AgentStats[] agentStats)
    {
        _agentStatsList = agentStats;

        CacheAgentStats();
        InitializeAgentLevels(0);
        CachePrefabNames();
    }
    
    private void CacheAgentStats()
    {
        foreach (var stats in _agentStatsList)
        {
            _agentStatsCache[stats.agentType] = stats;
        }
    }

    private void CachePrefabNames()
    {
        foreach (var stats in _agentStatsList)
        {
            if (stats.prefab != null)
            {
                _prefabNameCache[stats.agentType] = stats.prefab.name;             
            }
            else
            {
                Debug.LogError("No prefab");
            }
        }
    }

    public void Clear()
    {
        _availableAgentTypes.Clear();
        _agentLevels.Clear();
        _prefabNameCache.Clear();
        _agentStatsList = null;
    }

    public AgentType GetRandomAvailableAgentType()
    {
        if (_availableAgentTypes.Count == 0)
            return AgentType.None;
            
        return _availableAgentTypes[Random.Range(0, _availableAgentTypes.Count)];
    }
    
    private void InitializeAgentLevels(int level)
    {
        foreach (var agentStats in _agentStatsList)
        {
            _agentLevels[agentStats.agentType] = level;
        }
    }
    
    public bool IsAgentTypeAvailable(AgentType agentType)
    {
        return _availableAgentTypes.Contains(agentType);
    }
    
    public void AddAvailableAgentType(AgentType agentType)
    {
        if (!_availableAgentTypes.Contains(agentType))
        {
            _availableAgentTypes.Add(agentType);
        }
    }
    
    public void UpgradeAgentLevel(AgentType agentType)
    {
        if (_agentLevels.ContainsKey(agentType))
        {
            _agentLevels[agentType]++;
        }
    }
    
    public AgentController CreateAgent(AgentType type, Transform parent, Vector3 position, Team team, IAgentControllerListener listener)
    {
        AgentStats agentStats = GetAgentStats(type);
        
        if (agentStats == null)
        {
            Debug.LogError($"No stats");
            return null;
        }      

        if (!_prefabNameCache.TryGetValue(type, out string prefabName))
        {     
            Debug.LogError("No prefab");    
            return null;
        }        
     
        GameObject agentObj = ObjectPooler.Instance.Spawn(prefabName, position, parent);
        
        if (agentObj == null)
        {
            Debug.LogError("Failed to spawn agent from pool for type");
            return null;
        }        
       
        AgentController agentController = agentObj.GetComponent<AgentController>();
        AgentView agentView = agentObj.GetComponent<AgentView>();
        
        if (agentController == null || agentView == null)
        {
            Debug.LogError("missing required components");
            
            PoolObject poolObject = agentObj.GetComponent<PoolObject>();
            if (poolObject != null)             
                poolObject.GoToPool();     
            else
                Object.Destroy(agentObj);
            return null;
        }        
        
        int agentLevel = _agentLevels[type];
        LevelStats levelStats = agentStats.GetStatsForLevel(agentLevel);
        
        if (levelStats == null) return null;
       
        agentView.Initialize(5f, Camera.main, agentLevel);    

        agentController.Initialize(
            levelStats,      
            agentView,
            listener,
            levelStats.rank,
            team
        );
        
        return agentController;
    }    
   
    public void ReturnAgentToPool(AgentController agent)
    {
        if (agent == null) return;
        
        PoolObject poolObject = agent.GetComponent<PoolObject>();
        if (poolObject != null)
        {
            poolObject.GoToPool();
        }
        else
        {
            Debug.LogError("No poolObject component");
            Object.Destroy(agent.gameObject);
        }
    }
    
    private AgentStats GetAgentStats(AgentType type)
    {
        return _agentStatsCache.TryGetValue(type, out var stats) ? stats : null;
    }
}