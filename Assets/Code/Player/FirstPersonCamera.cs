using UnityEngine;

namespace Player
{
    // NOTE: also controls root object yaw axis
    public class FirstPersonCamera : MonoBehaviour
    {
        [SerializeField]
        private float _sensitivity = 2f;

        [SerializeField]
        private float _pitchMin = -80f;

        [SerializeField]
        private float _pitchMax = 80f;
        
        [field: SerializeField]
        public Camera Camera { get; private set; }

        public float Pitch { get; private set; }
        public float Yaw { get; private set; }

        private void LateUpdate()
        {
            Camera.transform.localRotation = Quaternion.Euler(Pitch, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, Yaw, 0f);
        }

        public void Rotate(float x, float y)
        {
            float sens = _sensitivity;

            Pitch -= y * sens;
            Pitch = Mathf.Clamp(Pitch, _pitchMin, _pitchMax);

            Yaw += x * sens;
        }

        public void SetLookDirection(Vector3 forward)
        {
            Vector3 euler = Quaternion.LookRotation(forward, Vector3.up).eulerAngles;
            Pitch = euler.x;
            Yaw = euler.y;
        }
    }
}