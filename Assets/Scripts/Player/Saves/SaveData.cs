using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    private static SaveData current;
    public static SaveData Instance
    {
        get
        {
            if (current == null)
                current = new SaveData();

            return current;
        }
        
        set
        {
            if (value != null)
                current = value;
        }
    }

    public PlayerData playerData;
    public List<string> itemDatas = new();
    public string keybindingsJson;
    public List<string> questData = new();
}
