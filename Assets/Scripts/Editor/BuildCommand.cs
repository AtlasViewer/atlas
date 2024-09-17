using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

static class BuildCommand
{
    private const string IS_DEVELOPMENT_BUILD = "IS_DEVELOPMENT_BUILD";
    private const string BUILD_OPTIONS_ENV_VAR = "buildOptions";

    

    public static void ProcessCLIBuild() {
        Console.WriteLine("> PERFORM BUILD");

        var buildTarget = GetBuildTarget();
        var targetGroup = GetBuildTargetGroup(buildTarget);

        var buildPath = GetBuildPath();
        var buildName = GetBuildName();
        var buildOpts = GetBuildOptions();
        var fixedPath = GetFixedBuildPath(buildTarget, buildPath, buildName);

        BuildPlayerOptions BPO = new BuildPlayerOptions();
        BPO.target = buildTarget;
        BPO.targetGroup = targetGroup;
        BPO.options = buildOpts;
        BPO.locationPathName = fixedPath;
        BPO.scenes = GetEnabledScenes();

        var buildReport = BuildPipeline.BuildPlayer(BPO);
        string reportStr = $"Build Report\n\n\n\nSTATUS : {buildReport.summary.result}\n\n\n\nSummary : {buildReport.summary}\n\n";
        File.WriteAllText("report.txt", reportStr);
        
        if(buildReport.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            throw new Exception($"Build ended with {buildReport.summary.result} status");

        Console.WriteLine("> BUILD DONE");
    }

    static bool IsDevelopmentType() {
        if(TryGetEnv(IS_DEVELOPMENT_BUILD, out string val)) {
            return bool.Parse(val);
        }

        Console.WriteLine("> NO DEV_ENV FLAG FOUND, DEFAULTING TO NO");
        return false;
    }

    private static void HandleDevelopmentBuildType(bool isDevBuild) {
        EditorUserBuildSettings.development = isDevBuild;
        PlayerSettings.SplashScreen.show = isDevBuild;
    }

    static BuildTarget GetBuildTarget() {
        string buildTgtName = GetArgument("buildTarget");

        if(buildTgtName.TryConvertToEnum(out BuildTarget tgt))
            return tgt;

        return BuildTarget.NoTarget;
    }

    static BuildTargetGroup GetBuildTargetGroup(BuildTarget tgt) {
        string tgtGroup;
        string platform = tgt.ToString();

        if(tgt.ToString().ToLower().Contains("standalone"))
            tgtGroup="Standalone";
        else if(tgt.ToString().ToLower().Contains("wsa"))
            tgtGroup = "WSA";
        else tgtGroup = platform;

        if (tgtGroup.TryConvertToEnum(out BuildTargetGroup tgtGrp))
            return tgtGrp;

        return BuildTargetGroup.Unknown;
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

    static string GetBuildPath()
    {
        string buildPath = GetArgument("customBuildPath");
        Console.WriteLine(":: Received customBuildPath " + buildPath);
        if (buildPath == "")
        {
            return ".\\output\\";
        }
        return buildPath;
    }

    static string GetBuildName()
    {
        string buildName = GetArgument("customBuildName");
        Console.WriteLine(":: Received customBuildName " + buildName);
        if (buildName == "")
        {
            return "PrivateBuild"; // A default name
        }
        return buildName;
    }

    static string GetFixedBuildPath(BuildTarget buildTarget, string buildPath, string buildName)
    {
        if (buildTarget.ToString().ToLower().Contains("windows")) {
            buildName += ".exe";
        } else if (buildTarget == BuildTarget.Android) {
            buildName += EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
        }
        return buildPath + buildName;
    }

    static BuildOptions GetBuildOptions()
    {
        if (TryGetEnv(BUILD_OPTIONS_ENV_VAR, out string envVar)) {
            string[] allOptionVars = envVar.Split(',');
            BuildOptions allOptions = BuildOptions.None;
            BuildOptions option;
            string optionVar;
            int length = allOptionVars.Length;

            Console.WriteLine($":: Detecting {BUILD_OPTIONS_ENV_VAR} env var with {length} elements ({envVar})");

            for (int i = 0; i < length; i++) {
                optionVar = allOptionVars[i];

                if (optionVar.TryConvertToEnum(out option)) {
                    allOptions |= option;
                }
                else {
                    Console.WriteLine($":: Cannot convert {optionVar} to {nameof(BuildOptions)} enum, skipping it.");
                }
            }

            return allOptions;
        }

        return BuildOptions.None;
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
