using Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Pause
{
    public class PauseMenu : SingletonMonoBehaviour<PauseMenu>
    {
        [SerializeField]
        private Canvas _canvas;

        public UnityEvent OnShow = new();
        public UnityEvent OnHide = new();

        protected override void Awake()
        {
            base.Awake();
            _canvas.gameObject.SetActive(false);
        }

        public void Show()
        {
            _canvas.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
            
            OnShow?.Invoke();
        }

        public void Hide()
        {
            _canvas.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            
            OnHide?.Invoke();
        }

        public bool IsVisible() => _canvas.gameObject.activeSelf;
        
        public void Toggle()
        {
            if (IsVisible())
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Resume() => Hide();

        public void QuitToMenu()
        {
            Hide();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}