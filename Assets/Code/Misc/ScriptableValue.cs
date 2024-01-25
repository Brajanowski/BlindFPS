using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    public abstract class ScriptableValue<T> : ScriptableObject
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        public UnityEvent<T> OnValueChanged = new();
    }
}