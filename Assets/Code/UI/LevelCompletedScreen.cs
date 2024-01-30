using Core;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LevelCompletedScreen : SingletonMonoBehaviour<LevelCompletedScreen>
    {
        [SerializeField]
        private RectTransform _screen;

        [SerializeField]
        private Button _nextLevelButton;

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
            _nextLevelButton.interactable = Game.Instance.IsThereNextLevel(Game.Instance.State.GameLevel);
        }

        public void Hide()
        {
            _screen.gameObject.SetActive(false);
        }

        public void SetTimer(float time)
        {
            _timerText.SetSeconds(time);
        }

        public void Next()
        {
            Game.Instance.StartLevel(Game.Instance.State.GameLevel + 1);
            Hide();
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