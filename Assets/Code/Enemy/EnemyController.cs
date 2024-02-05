using AI;
using UnityEngine;

namespace Enemy
{
    public class EnemyAIData
    {
        public Vector3 LastSeenPlayerPosition;
        public Vector3 LastHeardPlayerPosition;
    }

    public class EnemyController : MonoBehaviour
    {
        private BehaviourTree<EnemyAIData> _behaviourTree;

        private void OnEnable()
        {
            BehaviourTreeBuilder<EnemyAIData> treeBuilder = new BehaviourTreeBuilder<EnemyAIData>(new EnemyAIData());
            treeBuilder.BeginSelector()
                       .AddLeaf(new BTNodeWait<EnemyAIData>(3.0f))
                       .AddLeaf(new BTNodeTestRandomizePlayerPosition())
                       .AddLeaf(new BTNodeTestPrintLocation())
                       .End();

            _behaviourTree = treeBuilder.Build();
        }

        private void Update()
        {
            _behaviourTree.Tick();
        }
    }
}