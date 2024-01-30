using System.Collections;
using Core;
using UnityEngine;

namespace GameMode.Modes
{
    [CreateAssetMenu(menuName = BlindFPSConstants.AssetCreationMenu + "Game Mode/Main Menu")]
    public class ScriptableMainMenuGameMode : ScriptableGameMode
    {
        public override IEnumerator OnEnter()
        {
            yield return SceneLoader.Instance.LoadMainMenu();
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