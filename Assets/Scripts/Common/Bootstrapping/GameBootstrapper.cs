using Unity.FPS.Game;
using UnityEngine;

namespace Assets.Scripts.Common.Bootstrapping
{
    public static class GameBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            //TODO: implement VContainer and MessagePipe initialization
            //this is called only once per execution of the game so
            //we don't need any extra logic to avoid more than one initialization
        }
    }
}
