using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentStats", menuName = "Game/AgentStats")]
public class AgentStats : ScriptableObject
{
    [Header("Base Information")]
    public AgentType agentType;
    public AgentUnitType unitType;
    public GameObject prefab;
    
    [Header("Fixed Stats")]

    public float attackRange;
    public float attackSpeed;  
    public float chaseRange;
    
    [Header("Level Stats")]

    public List<LevelStats> levelStats;    
   
    public LevelStats GetStatsForLevel(int level)
    {
        if (level < 1) level = 1;        
      
        foreach (var stats in levelStats)
        {
            if (stats.level == level)
                return stats;
        } 
       
        return null;
    }   
    
    public float GetDamageForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.damage : 0;
    }    
  
    public float GetSpeedForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.moveSpeed : 0;
    }   
    
    public float GetHealthForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.health : 0;
    }    
   
    public float GetArmorForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.armor : 0;
    }    
   
    public Sprite GetIconForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.icon : null;
    }    

    public Sprite GetTitleForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.titleSprite : null;
    }

    public Sprite GetDescriptionForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.descriptionSprite : null;
    }
    public Sprite GetCardFrameForLevel(int level)
    {
        LevelStats stats = GetStatsForLevel(level);
        return stats != null ? stats.cardFrame : null;
    }   
   
    public int GetMaxLevel()
    {
        int maxLevel = 0;
        foreach (var stats in levelStats)
        {
            if (stats.level > maxLevel)
                maxLevel = stats.level;
        }
        return maxLevel;
    }
}