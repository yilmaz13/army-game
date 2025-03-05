public abstract class BaseValueController
{
    #region Private Members

    protected float _value;
    protected float _maxValue;

    #endregion

    #region Public Members
    public float Value => _value;
    public float MaxValue => _maxValue;

    #endregion

    #region Public Methods
    public virtual void Initialize(float value)
    {
        _value = value;
        _maxValue = value;
    }

    public void Repair(float amount)
    {
        _value += amount;
        if (_value > _maxValue)
        {
            _value = _maxValue;
        }
    }

    public void Reset()
    {
        _value = _maxValue;
    }

    #endregion
}
