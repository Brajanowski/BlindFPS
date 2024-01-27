using Core;
using UnityEngine;

namespace Player
{
    // NOTE: also controls root object yaw axis
    public class FirstPersonCamera : MonoBehaviour
    {
        [SerializeField]
        private ScriptableValue<float> _sensitivity;

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
            float sens = _sensitivity.Value;

            Pitch -= y * sens;
            Pitch = Mathf.Clamp(Pitch, _pitchMin, _pitchMax);

            Yaw += x * sens;
        }

        public void SetLookDirection(Vector3 forward)
        {
            Pitch = Mathf.Asin(-forward.y) * Mathf.Rad2Deg;
            Yaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        }
    }
}