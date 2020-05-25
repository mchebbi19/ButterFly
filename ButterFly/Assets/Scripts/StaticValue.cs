using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticValue
{
    // Start is called before the first frame update
    public static bool bEnableSound = Utility.ConvertIntToBool(PlayerPrefs.GetInt("EnableSound", 1));


    public const string PRivacyPolicyURL = "https://sites.google.com/view/megoldas-strategic/privacy-policy-butterfly?authuser=0";
}
