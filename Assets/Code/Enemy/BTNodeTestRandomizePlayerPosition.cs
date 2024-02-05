using AI;
using UnityEngine;

namespace Enemy
{
    public class BTNodeTestRandomizePlayerPosition : BTNode<EnemyAIData>
    {
        public override NodeStatus Execute()
        {
            _data.LastSeenPlayerPosition = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Debug.Log("Randomized position: " + _data.LastSeenPlayerPosition);
            return NodeStatus.Success;
        }

        public override NodeStatus Tick()
        {
            return NodeStatus.Success;
        }
    }
}