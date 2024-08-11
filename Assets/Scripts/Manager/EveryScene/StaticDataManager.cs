using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StaticDataManager : Singleton<StaticDataManager>
{
    [ReadOnly]
    public StaticData data;

    [SerializeField]
    string staticDataFileName = "systemData.json";
    
    void LoadData() {
        string path = Path.Combine(Application.persistentDataPath, staticDataFileName);
        if(!File.Exists(path)) {
            data = new StaticData();
            SaveData();
        }
        string jsonData = File.ReadAllText(path);
        data = JsonUtility.FromJson<StaticData>(jsonData);
    }

    public void SaveData() {
        string jsonData = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, staticDataFileName);
        File.WriteAllText(path, jsonData);
    }

    private void Awake() {
        LoadData();
        Screen.SetResolution(data.resolutionX, data.resolutionY, data.isFullScreen);
    }

}
