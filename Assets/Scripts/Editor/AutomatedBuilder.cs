using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;

namespace AutomatedBuilder
{
    public class AutomatedBuilder
    {
        private static string[] arguments = Environment.GetCommandLineArgs();

        private const string OUTPUT_PATH_PARAMETER_KEY = "-outputPath";

        [MenuItem("Build/Build Win64")]
        public static void BuildWin64()
        {
            string buildName = PlayerSettings.productName + ".exe";
            // TODO: clean build cache should be modified by command line args??
            Build(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, BuildOptions.CleanBuildCache | BuildOptions.StrictMode, buildName);
        }

#region Generic Build Method
        private static void Build(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup, BuildOptions buildOptions, string buildName)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetActiveScenesList(),
                locationPathName = GetOutputPathFromEnvironment(OUTPUT_PATH_PARAMETER_KEY, buildName),
                target = buildTarget,
                options = buildOptions
            };

            // TODO: obsoleted, we need to update this script
            PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetIl2CppCompilerConfiguration(buildTargetGroup, Il2CppCompilerConfiguration.Release);

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build failed");
            }
        }
#endregion

#region Helper methods
        private static string GetOutputPathFromEnvironment(string outputEnvironmentKey, string buildName)
        {
            string outputPathArgument = GetCommandLineArgument(outputEnvironmentKey);
            if (string.IsNullOrEmpty(outputPathArgument))
            {
                throw new ArgumentException("Path argument not found");
            }

            // DataPath is Asset folder, so we go one folder up with ..
            string projectBasePath = Path.Combine(Application.dataPath, "../");
            string fullPath = Path.Combine(projectBasePath, outputPathArgument);

            string directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Debug.Log($"Folder created: {directory}");
            }

            return Path.Combine(fullPath, buildName);
        }

        private static string GetCommandLineArgument(string argument)
        {
            string argumentValue = string.Empty;

            for (var index = 0; index < arguments.Length; index++)
            {
                if (arguments[index] == argument)
                {
                    var nextIndex = index + 1;

                    if ((nextIndex < arguments.Length) && (arguments[nextIndex][0] != '-'))
                    {
                        argumentValue = arguments[nextIndex];
                        break;
                    }
                }
            }

            return argumentValue;
        }

        private static string[] GetActiveScenesList()
        {
            return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        }
#endregion
    }
}
