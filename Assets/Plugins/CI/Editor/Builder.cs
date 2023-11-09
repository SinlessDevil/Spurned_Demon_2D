using System;
using System.Linq;
using Services.StaticData;
using StaticData;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Plugins.CI.Editor
{
  public static class Builder
  {
    [MenuItem("CI/Build/📦 Android APK Dev")]
    public static void BuildAndroidAPK_Dev()
    {
      PlayerSettings.stripEngineCode = true;
      DisableSplashScreen();
      SetStackTraceLogTypes(gameStaticData => gameStaticData.LogStackTraceDev);
      BuildApk(); 
    }

    [MenuItem("CI/Build/📦 Android APK Release")]
    public static void BuildAndroidAPK_Release()
    {
      PlayerSettings.stripEngineCode = true;
      DisableSplashScreen();
      SetStackTraceLogTypes(gameStaticData => gameStaticData.LogStackTraceRelease);
      BuildApk(); 
      BuildAab();
    }

    [MenuItem("CI/Build/🚚 Android Project")]
    public static void BuildAndroidProject()
    {
      EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
      BuildAndroid(Navigator.AndroidProjectPath());
    }


    [MenuItem("CI/Build/🍎 iOS Project Dev")]
    public static void BuildXCodeProject_Dev()
    {
      PlayerSettings.stripEngineCode = true;
      DisableSplashScreen();
      SetStackTraceLogTypes(gameStaticData => gameStaticData.LogStackTraceDev);

      PlayerSettings.iOS.appleEnableAutomaticSigning = false;
      PlayerSettings.iOS.appleDeveloperTeamID = "722HB3BL3F"; 
      BuildIOS(Navigator.ApkPath());
    }

    [MenuItem("CI/Build/🍎 iOS Project Release")]
    public static void BuildXCodeProject_Release()
    {
      PlayerSettings.stripEngineCode = true;
      DisableSplashScreen();
      SetStackTraceLogTypes(gameStaticData => gameStaticData.LogStackTraceRelease);

      PlayerSettings.iOS.appleEnableAutomaticSigning = false;
      PlayerSettings.iOS.appleDeveloperTeamID = "722HB3BL3F"; 
      BuildIOS(Navigator.ApkPath());
    }

    private static void BuildApk()
    {
      SetupKeystore();
      PlayerSettings.Android.useAPKExpansionFiles = false;
      EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
      EditorUserBuildSettings.buildAppBundle = false;
      BuildAndroid(Navigator.ApkPath());
    }

    private static void BuildAab()
    {
      SetupKeystore();
      PlayerSettings.Android.useAPKExpansionFiles = false;
      EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
      EditorUserBuildSettings.buildAppBundle = true;
      BuildAndroid(Navigator.AabPath());
    }

    private static void BuildAndroid(string locationPath)
    {
      BuildReport report = Build(new BuildPlayerOptions
      {
        target = BuildTarget.Android,
        locationPathName = locationPath,
        scenes = Scenes(),
      });

      if (report.summary.result != BuildResult.Succeeded)
        throw new Exception("Android Build Failed. See log for details");
    }

    private static void BuildIOS(string locationPath)
    {
      BuildReport report = Build(new BuildPlayerOptions
      {
        target = BuildTarget.iOS,
        locationPathName = locationPath,
        options = BuildOptions.CompressWithLz4HC,
        scenes = Scenes(),
      });

      if (report.summary.result != BuildResult.Succeeded)
        throw new Exception("iOS Build Failed. See log for details");
    }

    private static BuildReport Build(BuildPlayerOptions buildOptions) => 
      BuildPipeline.BuildPlayer(buildOptions);

    private static string[] Scenes() => 
      EditorBuildSettings.scenes
        .Where(x => x.enabled)
        .Select(x => x.path)
        .ToArray();

    private static void SetupKeystore()
    {
      PlayerSettings.Android.useCustomKeystore = false;
    }

    private static void SetStackTraceLogTypes(Func<GameStaticData, LogStackTrace> selectLogTrace)
    {
      StaticDataService staticData = new StaticDataService();
      staticData.LoadData();
      LogStackTrace stackTraceConfig = selectLogTrace(staticData.GameConfig);
      SetStackTraceLogTypes(stackTraceConfig.Info, stackTraceConfig.Errors);
    }

    private static void SetStackTraceLogTypes(StackTraceLogType info, StackTraceLogType errors)
    {
      PlayerSettings.SetStackTraceLogType(LogType.Log, info);
      PlayerSettings.SetStackTraceLogType(LogType.Warning, info);
      PlayerSettings.SetStackTraceLogType(LogType.Error, errors);
      PlayerSettings.SetStackTraceLogType(LogType.Exception, errors);
      PlayerSettings.SetStackTraceLogType(LogType.Assert, errors);
    }


    private static void DisableSplashScreen()
    {
      PlayerSettings.SplashScreen.show = false;
      PlayerSettings.SplashScreen.logos = Array.Empty<PlayerSettings.SplashScreenLogo>();
    }

  }
}
