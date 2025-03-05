using UnityEngine;

[CreateAssetMenu(fileName = "GameResources", menuName = "ScriptableObjects/GameResources", order = 1)]

public class GameResources : ScriptableObject
{
    //  MEMBERS    
    [SerializeField] private GameObject _ArmyControllerPrefab;
    [SerializeField] private AgentStats[] AgentStats;  
    [SerializeField] private AgentStats _castleStats;
    [SerializeField] private AgentStats _wallStats;

    [SerializeField] private ArmyLevelData _armyLevelData;

    public GameObject ArmyControllerPrefab => _ArmyControllerPrefab;

    public AgentStats GetAgentStats(AgentType agentType)
    {
        foreach (var agent in AgentStats)
        {
            if (agent.agentType == agentType)
            {
                return agent;
            }
        }
        return null;
    }

    public  AgentStats[] GetAgentStats()
    {
        return AgentStats;
    }

    public AgentStats GetCastleStats()
    {
        return _castleStats;
    }

    public ArmyLevelData GetArmyLevelData()
    {
        return _armyLevelData;
    }
}
