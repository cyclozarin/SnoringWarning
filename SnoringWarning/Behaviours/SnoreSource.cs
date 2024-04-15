using System.Reflection;
using UnityEngine;
using System.Linq;
using MyceliumNetworking;
using Steamworks;

namespace SnoringWarning.Behaviours
{
    [RequireComponent(typeof(AudioSource))]
    internal class SnoreSource : MonoBehaviour
    {
        private AudioSource _source = null!;
        private AudioClip _snoreClip = null!;
        private Player _player = null!;
        
        private void InitSource()
        {
            _source = GetComponent<AudioSource>();
            _source!.loop = true;
            _player = GetComponentInParent<Player>();
            UpdateSourceSettings();
        }

        private bool SourceInited()
        {
            return _source != null;
        }

        internal void PlaySnore() { MyceliumNetwork.RPC(SnoringWarning.PLUGIN_MYCELIUM_ID, nameof(RPC_PlaySnore), ReliableType.Reliable); }
        internal void StopSnore() { MyceliumNetwork.RPC(SnoringWarning.PLUGIN_MYCELIUM_ID, nameof(RPC_StopSnore), ReliableType.Reliable); }
        private void UpdateSourceSettings() { MyceliumNetwork.RPC(SnoringWarning.PLUGIN_MYCELIUM_ID, nameof(RPC_UpdateSourceSettings), ReliableType.Reliable); }

        [CustomRPC]
        internal void RPC_PlaySnore(RPCInfo info)
        {
            if (!SourceInited())
                InitSource();
            MyceliumNetwork.RPC(SnoringWarning.PLUGIN_MYCELIUM_ID, nameof(RPC_UpdateSourceSettings), ReliableType.Reliable);
            _player.refs.visor.SetEmission(0f);
            _source.Play();
            SnoringWarning.Instance.LogDebug($"Playing snoring at {SteamFriends.GetFriendPersonaName(info.SenderSteamID)}'s head. His visor emission is {_player.refs.visor.m_material.GetFloat(PlayerVisor.Emis)} tho.");
        }

        [CustomRPC]
        internal void RPC_StopSnore(RPCInfo info)
        {
            var visor = _player.refs.visor;
            visor.SetEmission(visor.m_startEmission);
            _source.Stop();
            SnoringWarning.Instance.LogDebug($"Stopping snoring at {SteamFriends.GetFriendPersonaName(info.SenderSteamID)}'s head. His visor emission is {_player.refs.visor.m_material.GetFloat(PlayerVisor.Emis)} tho.");
        }

        [CustomRPC]
        private void RPC_UpdateSourceSettings()
        {
            _source.clip = SnoringWarningSettings.Snore == 0 ? SnoringWarning.Instance.GetBundleAsset<AudioClip>("snore_mimimi") : SnoringWarning.Instance.GetBundleAsset<AudioClip>("snore_augh");
            _source.volume = SnoringWarningSettings.Volume;
            SnoringWarning.Instance.LogDebug($"Clip name: {_snoreClip.name}; Volume: {_source.volume}");
        }
    }
}
