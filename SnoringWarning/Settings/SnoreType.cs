﻿using Zorro.Settings;
using System.Collections.Generic;
using ContentSettings.API.Settings;

namespace SnoringWarning.Settings;
internal class SnoreType : EnumSetting, ICustomSetting
{
    private List<string> _choices => ["Silent", "Loud"];

    public override void ApplyValue()
    {
        SnoringWarningSettings.SetSnoreType(Value);
        SnoringWarning.Instance.LogDebug($"Key name: Snore type; Type: {SnoringWarningSettings.Snore}");
    }

    public override List<string> GetChoices() => _choices;

    public string GetDisplayName() => "Snore type";

    public override int GetDefaultValue() => 0;
}

