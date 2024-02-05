namespace AI
{
    public interface IHasChildNodes<TData> where TData : class
    {
        void AddChild(BTNode<TData> node);
    }
    
    public abstract class BTNode<TData> where TData : class
    {
        protected TData _data;

        public virtual void SetData(TData data)
        {
            _data = data;
        }

        public abstract NodeStatus Execute();
        public abstract NodeStatus Tick();
    }
}