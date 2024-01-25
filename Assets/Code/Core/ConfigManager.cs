using Misc;
using UnityEngine;

namespace Core
{
    public class ConfigManager : SingletonMonoBehaviour<ConfigManager>
    {
        [SerializeField]
        private ScriptableValue<float> _sensitivity;

        [SerializeField]
        private float _defaultSensitivityValue = 1.0f;

        private void OnEnable()
        {
            _sensitivity.Value = PlayerPrefs.GetFloat(BlindFPSConstants.ConfigSensitivityKey, _defaultSensitivityValue);
        }

        public float GetMouseSensitivity() => _sensitivity.Value;
        public void SetMouseSensitivity(float value)
        {
            _sensitivity.Value = value;
            PlayerPrefs.SetFloat(BlindFPSConstants.ConfigSensitivityKey, _sensitivity.Value);
        }
    }
}