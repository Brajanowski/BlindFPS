using AI;
using UnityEngine;

namespace Enemy
{
    public class BTNodeWait<TData> : BTNode<TData> where TData : class
    {
        private readonly float _duration;
        private float _timer;

        public BTNodeWait(float duration)
        {
            _duration = duration;
        }

        public override NodeStatus Execute()
        {
            _timer = _duration;
            Debug.Log("Waiting for: " + _timer);
            
            return NodeStatus.Running;
        }

        public override NodeStatus Tick()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0.0f)
            {
                return NodeStatus.Failure;
            }

            return NodeStatus.Running;
        }
    }
}