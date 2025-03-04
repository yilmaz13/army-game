using UnityEngine;

public interface IAgentControllerListener 
{   
    IDamageable NearestEnemy(Vector3 position, float AttackRange, Team currentTeam);
    void HandleAgentDied(IDamageable agent, IDamageable killer);

    void HandleAgentKilled(IDamageable victim); 

}
