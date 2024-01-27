using System.Collections;

namespace GameMode
{
    public interface IGameMode
    {
        IEnumerator OnEnter();
        IEnumerator OnExit();
        void Tick();
    }
}