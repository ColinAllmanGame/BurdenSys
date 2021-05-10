using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NarrativeConstants
{
    //this class exists for rough circumstantial checks "Is the character in this area?", where the Burden may only have access to limited contexts.

    public enum narrativeRegions
    {
        Whitefield,
        BrokenPier,
        Sarn,
        Thunderhead,
    }

    public const string WhitefieldName = "Whitefield";
    public const string BrokenPierName = "Broken Pier";
    public const string SarnName = "Sarn";
    public const string ThunderheadName = "Thunderhead";
}
