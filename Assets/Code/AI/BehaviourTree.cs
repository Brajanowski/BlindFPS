using UnityEngine;

namespace AI
{
    public class BehaviourTree<TData> where TData : class
    {
        private readonly BTNode<TData> _root;
        private NodeStatus _status;

        public TData Data { get; }

        public BehaviourTree(TData data, BTNode<TData> root)
        {
            if (data == null)
            {
                Debug.LogError("Data cannot be null");
                return;
            }

            if (root == null)
            {
                Debug.LogError("Root node cannot be null");
                return;
            }

            Data = data;
            _root = root;

            _status = NodeStatus.Success;
            _root.SetData(data);
        }

        public void Tick()
        {
            if (_status == NodeStatus.Running)
            {
                _status = _root.Tick();
            }
            else
            {
                _status = _root.Execute();
            }
        }
    }
}