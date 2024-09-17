using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;

public static class BuildCommand
{
    public static void PerformWindows() {
        ProcessBuild("out\\windows\\AtlasViewer-Win64.exe", BuildTarget.StandaloneWindows64);
    }
    public static void PerformLinux() {
        ProcessBuild("out\\linux\\AtlasViewer-Linux", BuildTarget.StandaloneLinux64);
    }
    public static void PerformMacOS() {
        ProcessBuild("out\\macOS\\AtlasViewer-MacOS.app", BuildTarget.StandaloneOSX);
    }
    public static void PerformAndroid() {
        ProcessBuild("out\\android\\AtlasViewer-Android.apk", BuildTarget.Android);
    }


    public static void ProcessBuild(string path, BuildTarget target) {
        var outputPath = GetArgument("outputPath");
        outputPath += $"\\{path}";

        var opts = new BuildPlayerOptions {
            scenes = GetEnabledScenes(),
            target = target,
            locationPathName = outputPath
        };

        BuildPipeline.BuildPlayer(opts);
    }

    static bool TryGetEnv(string key, out string val) {
        val = Environment.GetEnvironmentVariable(key);
        return !string.IsNullOrEmpty(val);
    }

    static string GetArgument(string name) {
        string[] args = Environment.GetCommandLineArgs();
        for(int i=0;i<args.Length; i++) {
            if(args[i].Contains(name)) return args[i+1];
        }

        return null;
    }


    public static bool TryConvertToEnum<TEnum>(this string strEnumValue, out TEnum value)
    {
        if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
        {
            value = default;
            return false;
        }

        value = (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        return true;
    }

    static string[] GetEnabledScenes()
    {
        return (
            from scene in EditorBuildSettings.scenes
            where scene.enabled
            where !string.IsNullOrEmpty(scene.path)
            select scene.path
        ).ToArray();
    }


}
