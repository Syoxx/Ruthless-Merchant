using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace RuthlessMerchant
{
    class BuildScript
    {
        static string[] SCENES = FindEnabledEditorScenes();

        static string APP_NAME = "RuthlessMerchant";
        static string TARGET_DIR = "C:/Users/Riku/Desktop/RuthlessMerchantDaily";

        static void PerformAllBuilds()
        {
            PerformWindowsBuild();
            //PerformMacOSXBuild();
            //PerformWebStreamingBuild();
            //PerformWebBuild();
            //PerformAndroidBuild();
            //PerformiOSBuild();
        }

        static void PerformWindowsBuild()
        {
            string target_dir = APP_NAME + ".exe";
            GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneWindows, BuildOptions.None);
        }

        #region OtherPlatforms
        /*
        static void PerformMacOSXBuild()
        {
            string target_dir = APP_NAME;
            GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
        }

        static void PerformLinuxBuild()
        {
            string target_dir = "linux";
            GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneLinux, BuildOptions.None);
        }

        static void PerformWebStreamingBuild()
        {
            string target_dir = "webstreaming";
            GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.WebPlayerStreamed, BuildOptions.None);
        }

        static void PerformWebBuild()
        {
            string target_dir = "web";
            GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.WebPlayer, BuildOptions.None);
        }

        static void PerformWebGLBuild()
        {
            string target_dir = "webgl";
            GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.WebGL, BuildOptions.None);
        }

        static void PerformAndroidBuild()
        {
            //Set the path to the Android SDK on the machine, since Unity cannot retain the state properly
            AndroidSDKFolder.Path = "${ANDROID_HOME}";
            string target_dir = APP_NAME + ".apk";
            GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.Android, BuildOptions.None);
        }

        static void PerformiOSBuild()
        {
            string target_dir = "iOS";
            //We do not build the xcodeproject in the target directory, since we do not want to archive the
            //entire xcode project, but instead build with XCode, then output the .ipa through Jenkins
            GenericBuild(SCENES, target_dir, BuildTarget.iOS, BuildOptions.None);
        } */
        #endregion

        private static string[] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                EditorScenes.Add(scene.path);
            }
            return EditorScenes.ToArray();
        }

        static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        }
    }
}
