using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VKLib
{
    public class AutoBuilder : ScriptableObject
    {
        private static readonly string[] SCENES = FindEnabledEditorScenes();
        public static string AppName;
        public static string BuildNumber; // IOS에서만 씀 
        public static string BundleVersion; // 안드로이드에서만 씀
        public static string GitHash;
        public static bool IsAbb;
        public static string TargetDir;

        // Use real app name here
        /* Anyway the App will have the name as configured within the Unity-Editor
           This Appname is just for the Folder in which to Build */

        [MenuItem("Build/Build Android")]
        public static void BuildAndroid()
        {
            AppName = GetArg("-appName");
            TargetDir = GetArg("-buildFolder");
            BundleVersion = GetArg("-bundleVersion");
            GitHash = GetArg("-gitHash");
            IsAbb = GetArg("-isABB") == "TRUE";

            Debug.Log("Jenkins-Build: APP_NAME: " + AppName + " TARGET_DIR: "
                      + TargetDir + " BUNDLE_VERSION: " + BundleVersion + " GitHash: " + GitHash + "isABB:" + IsAbb);

            GenericBuild(SCENES, $"{TargetDir}/{AppName}", BundleVersion, GitHash, BuildTargetGroup.Android, BuildTarget.Android, IsAbb, BuildOptions.None);
        }

        [MenuItem("Build/Build IOS")]
        public static void BuildIOS()
        {
            TargetDir = GetArg("-buildFolder");
            BuildNumber = GetArg("-buildNumber");
            GitHash = GetArg("-gitHash");

            Debug.Log(" TARGET_DIR: " + TargetDir + " BuildNumber: " + BuildNumber + " GitHash: " + GitHash);

            try
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

                //빌드 넘버는 젠킨스 잡 번호를 따라간다.
                var buildNumber = int.Parse(BuildNumber);

                //번들버전은 애플에 표시되는 버전번호이다. 
                //IOS에서 번들버전과 빌드넘버는 하나의 번들버전에 여러개의 빌드번호가 들어가는 식이다.
                //그러나 빌드번호를 넣어주기가 불편하기 때문에 하나로 통일해서 쓴다.
                //번들번호를 빌드번호를 이용해서 만들어 쓰는 식이다. (또한 빌드번호는 젠킨스 잡번호임)
                //이로서 고유한 번들버전과 빌드넘버가 1회 젠킨스 빌드 할 때 마다 생기게 된다. 
                PlayerSettings.bundleVersion = $"0.0.{BuildNumber}";
                PlayerSettings.iOS.buildNumber = buildNumber.ToString();

                //BuildPlayerOptions 으로 변경된.
                var opt = new BuildPlayerOptions();
                opt.locationPathName = TargetDir;
                opt.scenes = FindEnabledEditorScenes();
                opt.target = BuildTarget.iOS;
                opt.targetGroup = BuildTargetGroup.iOS;

                var report = BuildPipeline.BuildPlayer(opt);

                Debug.LogFormat("**** player : {0}", report);
                var summary = report.summary;
                Debug.LogFormat("**** summary.result : {0}", summary.result);
                if (summary.result == BuildResult.Succeeded)
                {
                    Debug.Log("**** Succeeded!");
                }
                else if (summary.result == BuildResult.Failed)
                {
                    Debug.Log("**** Failed!");
                    foreach (var step in report.steps)
                    {
                        foreach (var message in step.messages)
                        {
                            Debug.Log("****" + message);
                        }
                    }

                    throw new Exception("build failed!");
                }
                else if (summary.result == BuildResult.Cancelled)
                {
                    Debug.Log("**** Cancelled!");
                    throw new Exception("build Cancelled!");
                }
                else
                {
                    // Unknown
                    Debug.Log("**** Unknown!");
                    throw new Exception("build Unknown!");
                }
            }
            catch (Exception e)
            {
                Debug.Log("AutoBuild Exception! " + e);
                throw;
            }
        }


        private static string[] FindEnabledEditorScenes()
        {
            var EditorScenes = new List<string>();

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                EditorScenes.Add(scene.path);
            }

            return EditorScenes.ToArray();
        }

        private static void GenericBuild(string[] scenes, string buildFilePath, string bundleVersion, string _gitHash, BuildTargetGroup build_target_group, BuildTarget build_target
            , bool isABB, BuildOptions build_options)
        {
            try
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(build_target_group, BuildTarget.Android);
                PlayerSettings.keyaliasPass = GetArg("-keyaliasPass");
                PlayerSettings.keystorePass = GetArg("-keystorePass");

                Debug.LogFormat("**** keyaliasPass : {0}", PlayerSettings.keyaliasPass);
                Debug.LogFormat("**** keystorePass : {0}", PlayerSettings.keystorePass);

                //il2cpp 지원
                PlayerSettings.SetScriptingBackend(build_target_group, ScriptingImplementation.IL2CPP);
                //C++빌드할때 디버깅인지 릴리즈인지 체크.
                PlayerSettings.SetIl2CppCompilerConfiguration(build_target_group, Il2CppCompilerConfiguration.Release);
                //x86제외
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;

                //구글 앱번들 (aab). isABB가 false 일때는 APK로 빌드된다.
                EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
                EditorUserBuildSettings.buildAppBundle = isABB;
                //디버그 심볼의 생성 여부.
                EditorUserBuildSettings.androidCreateSymbolsZip = true;

                //구글에 올리는 번들의 숫자번호
                //왜 10 해주냐면, 본 코드가 젠킨스의 자동 빌드 잡 번호를 사용하기 때문에 실패하거나 한 것이 쌓이면 번호가 꼬일 수 있기 때문.
                var androidBundleId = int.Parse(bundleVersion) * 100;

                //앱 자체의 버전. 구글 번들버젼 + 깃해시의 짧은버젼을 쓴다.
                PlayerSettings.bundleVersion = $"0.0.{androidBundleId}_{_gitHash.Substring(0, 7)}";

                PlayerSettings.Android.bundleVersionCode = androidBundleId;
                //안쓰는 유니티 엔진코드 빼서 용량 줄이기
                PlayerSettings.stripEngineCode = true;

                var buildPlayerOptions = new BuildPlayerOptions();
                buildPlayerOptions.scenes = scenes;
                buildPlayerOptions.locationPathName = buildFilePath;
                buildPlayerOptions.target = build_target;

                //여러개의 옵션 넘기기 참고 : https://answers.unity.com/questions/689338/buildplayer-how-to-pass-multiple-options.html

                //디버깅 빌드를 꺼서 릴리즈로 만듬. 필요하면 아래 코드를 켜줘야 함.
                //buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging;

                Debug.LogFormat("**** app_target : {0}", buildFilePath);

                var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

                Debug.LogFormat("**** player : {0}", report);

                var summary = report.summary;

                Debug.LogFormat("**** summary.result : {0}", summary.result);

                if (summary.result == BuildResult.Succeeded)
                {
                    Debug.Log("**** Succeeded!");
                }
                else if (summary.result == BuildResult.Failed)
                {
                    Debug.Log("**** Failed!");
                    foreach (var step in report.steps)
                    {
                        foreach (var message in step.messages)
                        {
                            Debug.Log("****" + message);
                        }
                    }
                }
                else if (summary.result == BuildResult.Cancelled)
                {
                    Debug.Log("**** Cancelled!");
                }
                else
                {
                    // Unknown
                    Debug.Log("**** Unknown!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /**
         * Get Arguments from the command line by name
         */
        private static string GetArg(string name)
        {
            var args = Environment.GetCommandLineArgs();

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }

            return null;
        }
    }
}