using System.Collections.Generic;

namespace SnoringWarning;
internal class SnoringWarningSettings
{
    internal enum SnoreType { Silent, Loud };

    internal static float Volume;

    internal static SnoreType Snore { get; private set; }

    internal static void SetSnoreType(int snoreType)
    {
        var _intToSnoreType = new Dictionary<int, SnoreType>()
        {
            [0] = SnoreType.Silent,
            [1] = SnoreType.Loud,
        };
        Snore = _intToSnoreType[snoreType];
    }
}
