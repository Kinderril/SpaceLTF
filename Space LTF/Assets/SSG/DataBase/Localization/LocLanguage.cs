using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public enum LocalizationKeys
{
      eng = 0,
      rus = 0,
}

public class LocLanguage
{
    public LocalizationKeys LocalizationKey;
    private Dictionary<string, string> Dictionary;

    public string Get(string key)
    {
        if (Dictionary.TryGetValue(key, out var ans))
        {
            return ans;
        }

        return $"ERROR LOC {key}";
    }

//    public void Read(string path = "Localization")
//    {
//        if (Dictionary.Count > 0) return;
//
//        var textAssets = Resources.LoadAll<TextAsset>(path);
//
//        foreach (var textAsset in textAssets)
//        {
//            var text = ReplaceMarkers(textAsset.text);
//            var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");
//
//            foreach (Match match in matches)
//            {
//                text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
//            }
//
//            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
//            var languages = lines[0].Split(',').Select(i => i.Trim()).ToList();
//
//            for (var i = 1; i < languages.Count; i++)
//            {
//                if (!Dictionary.ContainsKey(languages[i]))
//                {
//                    Dictionary.Add(languages[i], new Dictionary<string, string>());
//                }
//            }
//
//            for (var i = 1; i < lines.Length; i++)
//            {
//                var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[comma]", ",").Replace("[newline]", "\n")).ToList();
//                var key = columns[0];
//
//                for (var j = 1; j < languages.Count; j++)
//                {
//                    Dictionary[languages[j]].Add(key, columns[j]);
//                }
//            }
//        }
//
//        AutoLanguage();
//    }
    private static string ReplaceMarkers(string text)
    {
        return text.Replace("[Newline]", "\n");
    }


}
