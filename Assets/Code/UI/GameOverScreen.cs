using Core;
using UnityEngine;
using Utils;

namespace UI
{
    public class GameOverScreen : SingletonMonoBehaviour<GameOverScreen>
    {
        [SerializeField]
        private RectTransform _screen;

        [SerializeField]
        private TimerText _timerText;

        protected override void Awake()
        {
            base.Awake();
            Hide();
        }

        public void Show()
        {
            _screen.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            _screen.gameObject.SetActive(false);
        }
        
        public void SetTimer(float time)
        {
            _timerText.SetSeconds(time);
        }

        public void Restart()
        {
            Game.Instance.StartLevel(Game.Instance.State.GameLevel);
            Hide();
        }

        public void QuitToMenu()
        {
            Game.Instance.GoToMainMenu();
            Hide();
        }
    }
}