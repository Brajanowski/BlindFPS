using UnityEngine;

namespace Rendering
{
    public enum BlindFPSCameraType
    {
        Main,
        Viewmodel
    }
    
    public class BlindFPSCameraData : MonoBehaviour
    {
        private Camera _camera;

        public Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponent<Camera>();
                }

                return _camera;
            }
        }
        
        [field: SerializeField]
        public BlindFPSCameraType CameraType { get; private set; }
    }
}