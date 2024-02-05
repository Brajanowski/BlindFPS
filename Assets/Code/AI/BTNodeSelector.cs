using System.Collections.Generic;

namespace AI
{
    public class BTNodeSelector<TData> : BTNode<TData>, IHasChildNodes<TData> where TData : class
    {
        private readonly List<BTNode<TData>> _children = new List<BTNode<TData>>();
        private int _current;
        private bool _isCurrentNodeRunning = false;

        public override NodeStatus Execute()
        {
            _current = 0;
            _isCurrentNodeRunning = false;
            return NodeStatus.Running;
        }

        public override NodeStatus Tick()
        {
            NodeStatus childStatus = _isCurrentNodeRunning ? _children[_current].Tick() : _children[_current].Execute();

            if (childStatus == NodeStatus.Success)
            {
                return NodeStatus.Success;
            }

            if (childStatus == NodeStatus.Failure)
            {
                _current++;
                _isCurrentNodeRunning = false;
                if (_current >= _children.Count)
                {
                    _current = 0;
                    return NodeStatus.Failure;
                }

                return NodeStatus.Running;
            }

            _isCurrentNodeRunning = true;
            return childStatus;
        }

        public void AddChild(BTNode<TData> node)
        {
            _children.Add(node);
        }

        public override void SetData(TData data)
        {
            base.SetData(data);

            foreach (BTNode<TData> child in _children)
            {
                child.SetData(data);
            }
        }
    }
}