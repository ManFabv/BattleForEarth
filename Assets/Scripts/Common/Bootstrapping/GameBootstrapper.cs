using Assets.Scripts.Common.UnityGameUtilities;
using UnityEngine;

namespace Assets.Scripts.Common.Bootstrapping
{
    public static class GameBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            //we don't need to do any initialization if we are only running tests
            if(IsRunningTests())
            {
                return;
            }
            //TODO: implement VContainer and MessagePipe initialization
            //this is called only once per execution of the game so
            //we don't need any extra logic to avoid more than one initialization
            
            ActivateVariableRefreshRateSupport();
        }

        private static void ActivateVariableRefreshRateSupport()
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }

        private static bool IsRunningTests()
        {
#if UNITY_EDITOR
            // if we are on editor, we look if the active scene is in build 
            // in the best case, test scenes should never be included in build
            // so this is an accurate way of telling if we are running unit tests
            return !UnityScenesGameUtilities.IsActiveSceneInBuild();
#else
            return false; //outside the editor, we want to always initialize the game packages
#endif
        }
    }
}
