using UnityEngine;

namespace SnoringWarning.Behaviours
{
    [RequireComponent(typeof(AudioSource))]
    internal class SnoreSource : MonoBehaviour
    {
        private AudioSource _source = null!;

        public void Start()
        {
            _source = GetComponent<AudioSource>();
            _source!.loop = true;
        }

        internal void UpdateSourceSettings()
        {
            _source.clip = SnoringWarningSettings.Snore == 0 ? SnoringWarning.Instance.GetBundleAsset<AudioClip>("snore_mimimi") : SnoringWarning.Instance.GetBundleAsset<AudioClip>("snore_augh");
            _source.volume = SnoringWarningSettings.Volume;
            SnoringWarning.Instance.LogDebug($"Clip name: {_source.clip.name}; Volume: {_source.volume}");
        }

        internal void PlaySnore() 
        { 
            UpdateSourceSettings();
            _source.Play(); 
        }

        internal void StopSnore() 
        {
            UpdateSourceSettings();
            _source.Stop(); 
        }
    }
}
