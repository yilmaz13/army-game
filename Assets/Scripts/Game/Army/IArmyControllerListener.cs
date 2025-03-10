using UnityEngine;

public interface IArmyControllerListener 
{
    void HandleAgentSpawned(IDamageable agent);
    void HandleAgentDied(IDamageable agent);
    IDamageable NearestEnemy(Vector3 position, float range, Team currentTeam);

    void OnDefeatCastle(Team team);

    bool IsGameStarted();
}
