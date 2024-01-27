using Core;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public void Play()
        {
            Game.Instance.StartLevel(0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}