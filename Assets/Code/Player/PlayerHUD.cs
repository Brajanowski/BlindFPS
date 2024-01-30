using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField]
        private TimerText _timerText;

        public void SetTimeElapsed(float seconds)
        {
            _timerText.SetSeconds(seconds);
        }
    }
}