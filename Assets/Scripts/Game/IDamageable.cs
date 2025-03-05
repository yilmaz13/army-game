using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(float damage, IDamageable shooter,  float armorPenetration = 100);

    Vector3 GetPosition();

    Team GetTeam();

    bool IsActive();
}