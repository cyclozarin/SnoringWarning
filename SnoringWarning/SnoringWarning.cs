using BepInEx;
using BepInEx.Logging;
using SnoringWarning.Hooks;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace SnoringWarning;
[ContentWarningPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, true)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, "1.0.0")]
[BepInDependency("RugbugRedfern.MyceliumNetworking", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("CommanderCat101.ContentSettings", BepInDependency.DependencyFlags.HardDependency)]
public class SnoringWarning : BaseUnityPlugin
{
    public static SnoringWarning Instance { get; private set; } = null!;

    internal new static ManualLogSource Logger { get; private set; } = null!;

    private static string snoreBundleName = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault((n) => n.EndsWith("snoringwarning"));

    private static AssetBundle _snoreBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(snoreBundleName));

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        new BedHook().Init();
        // On.Bed.Interact += MM_Prefix_AutoSleepy;
        LogInfo($"{MyPluginInfo.PLUGIN_NAME} ({MyPluginInfo.PLUGIN_GUID}) has loaded!");
    }

    // Debug function for making players always sleepy
    //private void MM_Prefix_AutoSleepy(On.Bed.orig_Interact orig, Bed self, Player player)
    //{
    //    self.RequestSleep(player);
    //}

    public void LogInfo(object message) => Logger.LogInfo(message);

    public void LogDebug(object message) => Logger.LogDebug(message);

    internal T GetBundleAsset<T>(string name) where T : UnityEngine.Object
    {
        return _snoreBundle.LoadAsset<T>(name);
    }
}
