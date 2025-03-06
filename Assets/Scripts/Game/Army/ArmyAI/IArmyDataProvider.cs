using System.Collections.Generic;
using UnityEngine;

public interface IArmyDataProvider
{
    int GetUnitCount();
    float GetArmyPower();
    float GetEnemyPower();
    Vector3 GetEnemyPosition();
    bool IsEnemyDefeated();

    List<ArmyController> GetEnemyArmies();
    
    List<int> GetUnitIDs(); // Tüm birim ID'leri
    List<int> GetTankUnitIDs(); // Sadece tank birimleri
    List<int> GetRangedUnitIDs(); // Sadece menzilli birimler

}