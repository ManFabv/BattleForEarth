using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Common.UnityGameUtilities
{
    public static class UnityScenesGameUtilities
    {
        /// <summary>
        /// We get all the scenes names that are setup on the editor settings and are also enabled
        /// </summary>
        /// <returns>list of the scene names that are enabled on build</returns>
        public static IEnumerable<string> GetAllActiveSceneNamesInBuild()
        {
#if UNITY_EDITOR
            IEnumerable<string> scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path);

            return scenes ?? new List<string>();
#else
            return new List<string>(); \\TODO: we should return something else for runtime?
#endif
        }

        /// <summary>
        /// we count all the scenes that are setup and enabled on the editor settings
        /// </summary>
        /// <returns>the number of active scenes that are included on the editor settings</returns>
        public static int GetAllActiveSceneCountInBuild()
        {
            return GetAllActiveSceneNamesInBuild().Count();
        }

        /// <summary>
        /// We check if the current scene is setup on the editor settings
        /// </summary>
        /// <returns>true if the current scene is setup in build</returns>
        public static bool IsActiveSceneInBuild()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            int activeSceneIndex = activeScene.buildIndex;
            int numberOfScenesOnBuild = GetAllActiveSceneCountInBuild();

            return activeSceneIndex >= 0 && activeSceneIndex < numberOfScenesOnBuild;
        }
    }
}
