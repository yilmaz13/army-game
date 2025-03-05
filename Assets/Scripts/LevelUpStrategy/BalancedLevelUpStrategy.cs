using System.Collections.Generic;

public class BalancedLevelUpStrategy : ILevelUpStrategy
{
    private ArmyController _army;
    private int _upgradeIndex = 0;
    private List<AgentType> _availableTypes;
    
    public BalancedLevelUpStrategy(ArmyController army)
    {
        _army = army;
        _availableTypes = new List<AgentType>();
    }
    
    public void ApplyLevelUp()
    {
        _availableTypes.Clear();     
      
        AgentType[] availableUpgrades = _army.GetUpgradeOptionsForCurrentLevel();    

        if (availableUpgrades.Length == 0) return;
        _army.AddAvailableAgentType(availableUpgrades[0]);     
        
        _upgradeIndex++;
    }
}