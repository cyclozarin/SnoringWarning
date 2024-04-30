using SnoringWarning.Behaviours;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SnoringWarning.Hooks;
public class BedHook
{
    internal void Init()
    {
        On.Bed.RPCA_AcceptSleep += MM_Postfix_BedSnoringOn;
        On.Bed.RPCA_LeaveBed += MM_Prefix_BedSnoringOff;
        On.Player.Start += MM_Postfix_CreateSnoreSourceAtPlayer;
        SnoringWarning.Instance.LogDebug("Hooked bed and player");
    }

    private static bool IsPlayerHaveSnoreSource(Player player)
    {
        return player.refs.headPos.GetComponentInChildren<SnoreSource>() != null;
    }

    private IEnumerator MM_Postfix_CreateSnoreSourceAtPlayer(On.Player.orig_Start orig, Player self)
    {
        yield return orig(self);
        if (!IsPlayerHaveSnoreSource(self))
        {
            var snoreSourceObject = Object.Instantiate(new GameObject("SnoreSource"), self.HeadPosition(), Quaternion.identity, self.refs.headPos.transform);
            var snoreSource = snoreSourceObject.AddComponent<SnoreSource>();
            snoreSource.UpdateSourceSettings();
            snoreSource.hideFlags = HideFlags.HideAndDontSave;
            Object.DontDestroyOnLoad(snoreSourceObject);
            SnoringWarning.Instance.LogDebug("Snore source instantiated");
        }
    }

    private static void MM_Postfix_BedSnoringOn(On.Bed.orig_RPCA_AcceptSleep orig, Bed self, int playerID)
    {
        orig(self, playerID);
        self.playerInBed.refs.headPos.GetComponentInChildren<SnoreSource>().PlaySnore();
        SnoringWarning.Instance.LogDebug("Playing snore");
    }

    private static void MM_Prefix_BedSnoringOff(On.Bed.orig_RPCA_LeaveBed orig, Bed self)
    {
        self.playerInBed.refs.headPos.GetComponentInChildren<SnoreSource>().StopSnore();
        SnoringWarning.Instance.LogDebug("Stopping snore");
        orig(self);
    }
}
