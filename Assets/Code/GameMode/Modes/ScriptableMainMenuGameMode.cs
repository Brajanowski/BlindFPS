using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMode.Modes
{
    [CreateAssetMenu(menuName = BlindFPSConstants.AssetCreationMenu + "Game Mode/Main Menu")]
    public class ScriptableMainMenuGameMode : ScriptableGameMode
    {
        public override IEnumerator OnEnter()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            yield break;
        }

        public override IEnumerator OnExit()
        {
            yield break;
        }

        public override void Tick()
        {
        }
    }
}