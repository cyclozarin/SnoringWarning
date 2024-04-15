using Zorro.Settings;
using ContentSettings.API.Settings;
using UnityEngine;
using Unity.Mathematics;

namespace SnoringWarning.Settings;
internal class SnoreVolume : FloatSetting, ICustomSetting
{
    public override void ApplyValue()
    {
        SnoringWarningSettings.Volume = Value;
        SnoringWarning.Instance.LogDebug($"Key name: Snore volume; Volume: {SnoringWarningSettings.Volume} ({Mathf.Round(SnoringWarningSettings.Volume * 100f)}%)");
    }

    public override float GetDefaultValue() => 0.5f;

    public string GetDisplayName() => "Snore volume";

    public override float2 GetMinMaxValue() => new(0.01f, 1f);
}

