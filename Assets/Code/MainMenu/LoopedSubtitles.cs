using System;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    [Serializable]
    public struct SubtitleEntry
    {
        public string Text;
        public float Duration;
    }

    public class LoopedSubtitles : MonoBehaviour
    {
        [SerializeField]
        private SubtitleEntry[] _entries;

        [SerializeField]
        private TextMeshProUGUI _text;

        private float _timer;
        private int _currentSubtitle;

        private void OnEnable()
        {
            SetSubtitle(0);
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0.0f)
            {
                SetSubtitle((_currentSubtitle + 1) % _entries.Length);
            }
        }

        private void SetSubtitle(int index)
        {
            _currentSubtitle = index;
            _timer = _entries[index].Duration;
            _text.text = _entries[index].Text;
        }
    }
}