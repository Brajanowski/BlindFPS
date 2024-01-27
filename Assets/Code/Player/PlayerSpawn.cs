using UnityEngine;

namespace Player
{
    public struct PlayerSpawnLocation
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
    
    public class PlayerSpawn : MonoBehaviour
    {
        public virtual PlayerSpawnLocation GetSpawnLocation()
        {
            PlayerSpawnLocation spawnLocation = new()
            {
                Position = transform.position,
                Rotation = transform.rotation
            };
            return spawnLocation;
        }
    }
}