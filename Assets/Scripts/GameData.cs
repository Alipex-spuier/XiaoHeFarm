using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData :MonoBehaviour 
{
    #region Filds
    [SerializeField] string gold;
    [SerializeField] string buildname;
    [SerializeField] string cropname;
    [System.Serializable] class SaveData
    {
        public string gold;
        public string buildname;
        public string cropname;
        public Vector3 buildPosition;
        public Vector3 cropPosition;
    }
    const string GAME_DATA_KEY = "GameData";
    const string GAME_DATA_FILE_NAME = "GAMEDATA.save";
    #endregion
    #region Save and Load
    public void Save()
    {
        SaveByJson();
    }
    public void Load()
    {
        LoadFromJson();
    }
    #endregion
    #region Json
    void SaveByJson()
    {
        SaveSystem.SaveByJson($"{System.DateTime.Now:yyyy.dd.M HH-mm-ss}.sav",SavingData()); 
    }
    void LoadFromJson()
    {
        var saveData=SaveSystem.LoadFromJson<SaveData>(GAME_DATA_FILE_NAME);
        LoadData(saveData); 
    }
    #endregion
    #region Help Functions

    SaveData SavingData()
    {
        var saveData = new SaveData();
        saveData.gold = gold;
        saveData.buildname = buildname;
        saveData.cropname = cropname;
        saveData.buildPosition = transform.position;
        saveData.cropPosition = transform.position;
        return saveData;
    }
    void LoadData(SaveData saveData)
    {
        gold = saveData.gold;
        buildname = saveData.buildname;
        cropname = saveData.cropname;
        transform.position = saveData.buildPosition;
        transform.position = saveData.cropPosition;
        
    }
    #endregion
}
