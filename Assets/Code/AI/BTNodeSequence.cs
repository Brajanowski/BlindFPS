using System.Collections.Generic;

namespace AI
{
    public class BTNodeSequence<TData> : BTNode<TData>, IHasChildNodes<TData> where TData : class
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
            BTNode<TData> currentChild = _children[_current];
            NodeStatus executeStatus = _isCurrentNodeRunning ? currentChild.Tick() : currentChild.Execute();

            if (executeStatus == NodeStatus.Failure)
            {
                return NodeStatus.Failure;
            }

            if (executeStatus == NodeStatus.Success)
            {
                _current++;
                _isCurrentNodeRunning = false;

                if (_current >= _children.Count)
                {
                    return NodeStatus.Success;
                }

                return NodeStatus.Running;
            }

            _isCurrentNodeRunning = true;
            return NodeStatus.Running;
        }

        public void AddChild(BTNode<TData> node)
        {
            _children.Add(node);
            node.SetData(_data);
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