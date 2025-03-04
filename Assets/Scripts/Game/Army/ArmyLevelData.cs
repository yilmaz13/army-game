
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmyLevelData", menuName = "Game/Army/Level Data")]
public class ArmyLevelData : ScriptableObject
{
    [Header("XP Settings")]   
    public List<int> xpRequirements = new List<int>();   
    
    [Header("Kill Rewards")]  
    public int xpPerRegularKill = 10;   
 
    public int xpPerEliteKill = 25;    
    
    public int xpPerBossKill = 100;
    
    [Header("Level Up Options")]   
    public List<LevelUpPool> levelUpPools = new List<LevelUpPool>();   
   
    public List<AgentTypeUpgradeData> agentUpgrades = new List<AgentTypeUpgradeData>();    
 
    public int GetXPForLevel(int level)
    {        
        if (level <= 0) return 0;        
 
        if (level <= xpRequirements.Count)
        {
            return xpRequirements[level - 1];
        }        
       
        int lastDefinedXP = xpRequirements[xpRequirements.Count - 1];
        int levelsAfterList = level - xpRequirements.Count;        
      
        for (int i = 0; i < levelsAfterList; i++)
        {
            lastDefinedXP = Mathf.RoundToInt(lastDefinedXP * 1.5f);
        }
        
        return lastDefinedXP;
    }    

    public int GetXPForKill(UnitRank rank)
    {
        switch (rank)
        {
            case UnitRank.Elite:
                return xpPerEliteKill;
            case UnitRank.Boss:
                return xpPerBossKill;
            case UnitRank.Regular:
            default:
                return xpPerRegularKill;
        }
    }
 
    public AgentType[] GetUpgradeOptionsForLevel(int level)
    {       
        LevelUpPool pool;
        
        if (level <= levelUpPools.Count)
        {
            pool = levelUpPools[level - 1];
        }
        else
        {           
            pool = levelUpPools[levelUpPools.Count - 1];
        }
        
        return pool.GetRandomOptions(3);
    }    

    public AgentTypeUpgradeData GetUpgradeDataForAgentType(AgentType type)
    {
        foreach (var data in agentUpgrades)
        {
            if (data.agentType == type)
            {
                return data;
            }
        }
        
        return new AgentTypeUpgradeData { agentType = type };
    }
}