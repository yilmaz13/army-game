using System.Collections.Generic;

public class AttackFocusedStrategy : ILevelUpStrategy
{
    private ArmyController _army;
    
    public AttackFocusedStrategy(ArmyController army)
    {
        _army = army;
    }
    
    public void ApplyLevelUp()
    {
        List<AgentType> attackTypes = new List<AgentType> 
        { 
            AgentType.Giant,
            AgentType.Warrior, 
            AgentType.Swordsman 
        };
        
        foreach (var type in attackTypes)
        {
            _army.UpgradeAgentType(type);
            return; 
        }
    }
}