
using System;

public class HealthController : BaseValueController
{
    public Action<IDamageable> OnDead;
    public Action OnHealthHalf;   
    public Action OnHealthThreeQuarter;
    public Action OnHealthTenPercent;    

    public bool isDead;
    #region Public Methods
    
    public override void Initialize(float value)
    {
        base.Initialize(value);
        isDead = false;       
    }    

    public void TakeDamage(float damage, IDamageable shooter)
    {
        if (!isDead)
        {
            _value -= damage;
            
            if (_value <= _maxValue / 2 && _value > _maxValue / 4)
            {
                OnHealthHalf?.Invoke();
            }
            else if (_value <= _maxValue / 4)
            {
                OnHealthThreeQuarter?.Invoke();
            }          

            if (_value <= 0)
            { 
                 Dead(shooter);
            }
        }
    }    

    #endregion

    #region Private Methods
    private void Dead(IDamageable killer)
    {
        _value = 0;
        OnDead(killer);
        isDead = true;
    }

    #endregion
}
