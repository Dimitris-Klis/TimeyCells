using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationSystem : MonoBehaviour
{
    public static LocalizationSystem instance;

    public TMP_Dropdown LanguageDropdown;
    [Space]
    public bool DebugDictionary;
    public bool DebugObjects;
    public int SelectedLanguage = 0;
    [Space]
    public string[] languageNames;
    public TextAsset[] textAssets;
    public Dictionary<string, string> stringPairs = new();

    
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void Setup()
    {
        textAssets = Resources.LoadAll<TextAsset>("Localization");
        
        //Get Language Names from the first line of each txt file.
        List<string> Languages = new List<string>();
        for (int i = 0; i < textAssets.Length; i++)
        {
            int end = textAssets[i].text.IndexOf("\r\n");
            Languages.Add(textAssets[i].text.Substring(0, end).Replace("Language Name: ", ""));
        }
        languageNames = Languages.ToArray();

        if(LanguageDropdown != null)
        {
            LanguageDropdown.ClearOptions();
            LanguageDropdown.AddOptions(Languages);
            LanguageDropdown.RefreshShownValue();
            LanguageDropdown.SetValueWithoutNotify(0);
        }
        else
        {
            Debug.LogWarning("You forgot to assign a dropdown to the Localization System!");
        }
    }
    public void SetLanguage(int langIndex)
    {
        stringPairs.Clear();
        string language = textAssets[langIndex].text;

        int startIndex = 0;
        int endIndex;

        //Unity is known to crash with infinite while loops. The failsafe is there to prevent this.
        //Messing up the txt key-value formatting could also cause an infinite while loop, which is why I've kept the failsafe.
        //If you feel confident, you can remove it.
        int failsafe = 0;

        string key = "";
        string text = "";

        //Removing Language Name
        endIndex = language.IndexOf("\r\n");
        language = language.Remove(startIndex, endIndex + 2);

        while (failsafe < 99 && language.Length > 0)
        {
            //Obtaining Key
            endIndex = language.IndexOf("\r\n");
            key = language.Substring(startIndex, endIndex);

            //Every time the key length is 0, that means there's more than 1 empty line.
            //So, we remove 1 until the length isn't 0.
            while (key.Length == 0)
            {
                language = language.Remove(startIndex, 2);
                endIndex = language.IndexOf("\r\n");
                key = language.Substring(startIndex, endIndex);
            }
            language = language.Remove(startIndex, endIndex + 2);

            //Obtaining Value
            endIndex = language.IndexOf("\r\n\r\n");
            if (endIndex == -1) endIndex = language.Length;
            text = language.Substring(startIndex, endIndex);
            language = language.Remove(startIndex, endIndex == language.Length ? endIndex : endIndex + 4);

            //Add new keyValuePair to dictionary.
            stringPairs.Add(key, text);

            failsafe++;
        }
        SelectedLanguage = langIndex;
        UpdateLocalizers();
        if (DebugDictionary)
        {
            foreach (KeyValuePair<string, string> item in stringPairs)
            {
                Debug.Log($"{item.Key}: {item.Value}");
            }
        }
    }
    public string GetText(string name, string key)
    {
        if(DebugObjects) Debug.Log(name + ": " + key);
        return stringPairs[key];
    }
    void UpdateLocalizers()
    {
        foreach(TMPLocalizer loc in FindObjectsByType<TMPLocalizer>(FindObjectsInactive.Include,FindObjectsSortMode.None))
        {
            loc.UpdateText();
        }
        foreach (TMPDropdownLocalizer loc in FindObjectsByType<TMPDropdownLocalizer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            loc.UpdateText();
        }
    }
}