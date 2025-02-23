namespace Army.State
{
    public abstract class AStateBase
    {
        public readonly string name;

        //  CONSTRUCTORS
        public AStateBase(string name)
        {
            this.name = name;
        }

        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void UpdateState();
    }
}