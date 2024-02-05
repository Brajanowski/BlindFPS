using AI;
using UnityEngine;

namespace Enemy
{
    public class BTNodeTestPrintLocation : BTNode<EnemyAIData>
    {
        public override NodeStatus Execute()
        {
            Debug.Log("LOCATION: " + _data.LastSeenPlayerPosition);
            return NodeStatus.Success;
        }

        public override NodeStatus Tick()
        {
            return NodeStatus.Success;
        }
    }
}