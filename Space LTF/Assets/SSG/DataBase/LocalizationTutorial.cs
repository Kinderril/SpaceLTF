using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LocalizationTutorial
{
    private static Dictionary<string, string> _locKeys = new Dictionary<string, string>();
    static LocalizationTutorial()
    {
        _locKeys.Add("battleSpells", $"Main ship special options. There you can choose what you want to use. Any option will use X count of charges for Y seconds.");
        _locKeys.Add("battleStart", $"Battle starts. All your ships will choose target and attack by self. You can control your main ship. To choose it select it on map, or use hotkey 'Q'");
        _locKeys.Add("mapUpgrade", "One of your ships can be upgraded. Click here to upgrade it.");
        _locKeys.Add("mapInventory", "This is your fleet page. Here you can change equipment of your ships, or do some upgrades.");
        _locKeys.Add("mapMain","This is global map. There you move to different places of galaxy. You can move only to close places. To move click to point you want to go.");
    }

    public static string GetKey(string id)
    {
        if (_locKeys.TryGetValue(id, out var txt))
        {
            return txt;
        }

        return $"ERROR:{id}";

    }
}
