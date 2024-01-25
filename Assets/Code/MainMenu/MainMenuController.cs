using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}