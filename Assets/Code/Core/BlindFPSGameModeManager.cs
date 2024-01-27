using GameMode;

namespace Core
{
    public enum BlindFPSGameMode
    {
        MainMenu,
        LevelSpeedrun
    }

    public class BlindFPSGameModeManager : GameModeManager<BlindFPSGameMode>
    {
    }
}