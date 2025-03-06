// Soyut arayüz
using System.Collections.Generic;

public interface IArmyInformation
{
    ArmyController GetArmyByTeam(Team team);    
    public List<ArmyController> GetEnemyArmies();

    List<ArmyController> GetArmiesByTeam(Team team);
    
    bool IsTeamDefeated(Team team);   

}