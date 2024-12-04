using System.Collections.Generic;
using UnityEngine;

public class PreferenceManager : GenericSingletonClass<PreferenceManager>
{
    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    public void SetBool(string pKey, bool pValue)
    {
        PlayerPrefs.SetInt(pKey, pValue ? 1 : 0);
    }

    public bool GetBool(string pKey, bool pDefaultValue = false)
    {
        return PlayerPrefs.GetInt(pKey, pDefaultValue ? 1 : 0) == 1;
    }

    public void SetString(string pKey, string pValue)
    {
        PlayerPrefs.SetString(pKey, pValue);
    }

    public string GetString(string pKey, string pDefaultValue = "")
    {
        return PlayerPrefs.GetString(pKey, pDefaultValue);
    }

    public void SetInt(string pKey, int pDefaultValue)
    {
        PlayerPrefs.SetInt(pKey, pDefaultValue);
    }

    public int GetInt(string pKey, int pDefaultValue = 0)
    {
        return PlayerPrefs.GetInt(pKey, pDefaultValue);
    }

    public void SetStringList(string key, List<string> stringList)
    {
        // Join the list into a single string, using '|' as a delimiter
        string serializedList = string.Join("|", stringList);
        PlayerPrefs.SetString(key, serializedList);
        PlayerPrefs.Save(); // Save changes
    }

    // Get a list of strings from preferences by splitting the stored string
    public List<string> GetStringList(string key)
    {
        // Retrieve the serialized list
        string serializedList = PlayerPrefs.GetString(key, "");

        // If the stored string is empty, return an empty list
        if (string.IsNullOrEmpty(serializedList))
        {
            return new List<string>();
        }

        // Split the string back into a list using '|' as a delimiter
        return new List<string>(serializedList.Split('|'));
    }
    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
