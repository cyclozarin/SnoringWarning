using ContentSettings.API.Settings;
using ContentSettings.API.Attributes;

namespace SnoringWarning.Settings;
[SettingRegister("CYCLOZARIN MODS", "SnoringWarning settings")]
internal class SnoreVolume : IntSetting, ICustomSetting
{
    public override void ApplyValue()
    {
        SnoringWarningSettings.Volume = Value / 100f;
        SnoringWarning.Instance.LogDebug($"Key name: Snore volume; Volume: {Value}% ({SnoringWarningSettings.Volume})");
    }

    protected override int GetDefaultValue() => 50; // "why is it outputing 49 instead of 50 in game?" you might ask. the answer is: idfk, ask dhkatz ig

    public string GetDisplayName() => "Snore volume";

    protected override (int, int) GetMinMaxValue() => (0, 100);
}

