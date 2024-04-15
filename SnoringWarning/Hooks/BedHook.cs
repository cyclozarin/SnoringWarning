using MyceliumNetworking;
using SnoringWarning.Behaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SnoringWarning.Hooks;
public class BedHook
{
    internal void Init()
    {
        MyceliumNetwork.RegisterNetworkObject(this, SnoringWarning.PLUGIN_MYCELIUM_ID);
        On.Bed.RPCA_AcceptSleep += MM_Postfix_BedSnoringOn;
        On.Bed.RPCA_LeaveBed += MM_Prefix_BedSnoringOff;
        SnoringWarning.Instance.LogDebug("Hooked bed");
    }
    private static bool IsPlayerHaveSnoreSource(Player player)
    {
        return player.refs.headPos.GetComponentInChildren<SnoreSource>() != null;
    }

    [CustomRPC]
    private void RPC_CreateSnoreSourceAtPlayer(int playerViewId)
    {
        Player player = PlayerHandler.instance.TryGetPlayerFromViewID(playerViewId);
        var snoreSourceObject = Object.Instantiate(new GameObject("SnoreSource"), player.HeadPosition(), Quaternion.identity, player.refs.headPos.transform);
        var snoreSource = snoreSourceObject.AddComponent<SnoreSource>();
        snoreSource.hideFlags = HideFlags.HideAndDontSave;
        Object.DontDestroyOnLoad(snoreSourceObject);
        MyceliumNetwork.RegisterNetworkObject(snoreSource, SnoringWarning.PLUGIN_MYCELIUM_ID);
        SnoringWarning.Instance.LogDebug("Snore source instantiated via RPC");
    }

    private static void MM_Postfix_BedSnoringOn(On.Bed.orig_RPCA_AcceptSleep orig, Bed self, int playerID)
    {
        orig(self, playerID);
        if (!IsPlayerHaveSnoreSource(self.playerInBed))
        {
            SnoringWarning.Instance.LogDebug($"{self.playerInBed.refs.view.Owner.NickName} doesnt have a snore source, we'll create it");
            MyceliumNetwork.RPC(SnoringWarning.PLUGIN_MYCELIUM_ID, nameof(RPC_CreateSnoreSourceAtPlayer), ReliableType.Reliable, self.playerInBed.refs.view.ViewID);
        }
        self.playerInBed.refs.headPos.GetComponentInChildren<SnoreSource>().PlaySnore();
    }
    private static void MM_Prefix_BedSnoringOff(On.Bed.orig_RPCA_LeaveBed orig, Bed self)
    {
        self.playerInBed.refs.headPos.GetComponentInChildren<SnoreSource>().StopSnore();
        orig(self);
    }
}
