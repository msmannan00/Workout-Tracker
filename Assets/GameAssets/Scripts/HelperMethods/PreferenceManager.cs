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
    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
