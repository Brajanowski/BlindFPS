using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    public class OptionSensitivity : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _valueText;

        [SerializeField]
        private Slider _slider;

        private void OnEnable()
        {
            _slider.value = ConfigManager.Instance.GetMouseSensitivity();
            _slider.onValueChanged.AddListener(OnValueChanged);
            UpdateValueText();
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            ConfigManager.Instance.SetMouseSensitivity(value);
            UpdateValueText();
        }

        private void UpdateValueText()
        {
            _valueText.text = _slider.value.ToString("F2");
        }
    }
}