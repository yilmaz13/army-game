
[System.Serializable]

public class AgentTypeUpgradeData
{
    public AgentType agentType;     
    public string upgradeDescription = "Units improved!";
}

public enum UnitRank
{
    Regular,
    Elite,
    Boss
}