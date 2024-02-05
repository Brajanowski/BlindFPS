using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class BehaviourTreeBuilder<TData> where TData : class
    {
        private Stack<BTNode<TData>> _nodeStack = new Stack<BTNode<TData>>();
        private TData _data;

        private BTNode<TData> _root;
        
        public BehaviourTreeBuilder(TData data)
        {
            _data = data;
        }

        public BehaviourTree<TData> Build()
        {
            return new BehaviourTree<TData>(_data, _root);
        }

        public BehaviourTreeBuilder<TData> BeginSequence()
        {
            _nodeStack.Push(new BTNodeSequence<TData>());
            return this;
        }

        public BehaviourTreeBuilder<TData> BeginSelector()
        {
            _nodeStack.Push(new BTNodeSelector<TData>());
            return this;
        }

        public BehaviourTreeBuilder<TData> End()
        {
            BTNode<TData> node = _nodeStack.Pop();

            if (_nodeStack.Count == 0)
            {
                _root = node;
            }
            else
            {
                AddLeaf(node);
            }
            
            return this;
        }

        public BehaviourTreeBuilder<TData> AddLeaf(BTNode<TData> leaf)
        {
            if (_nodeStack.Count == 0)
            {
                Debug.LogError("Root node is not defined! It must be either sequence or selector!");
                return null;
            }

            BTNode<TData> node = _nodeStack.Peek();

            if (node is IHasChildNodes<TData> hasChildNodes)
            {
                hasChildNodes.AddChild(leaf);
                return this;
            }
            
            Debug.LogError("Current node on stack doesn't implement IHasChildNodes interface!");
            return null;
        }
    }
}