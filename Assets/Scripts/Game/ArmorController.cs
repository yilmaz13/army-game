public class ArmorController : BaseValueController
{
    #region Public Methods
    public float AbsorbDamage(float damage, float armorPenetration)
    {
        float damageToArmor = damage * ((100 - armorPenetration) / 100);
        float remainingDamage = damage - damageToArmor;

        if (_value > 0)
        {
            if (_value >= damageToArmor)
            {
                _value -= damageToArmor;
            }
            else
            {
                remainingDamage += (damageToArmor - _value);
                _value = 0;
            }
        }

        return remainingDamage;
    }
    #endregion
}
