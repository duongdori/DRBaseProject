using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class DataManager : MonoSingleton<DataManager>
{

    public List<GameAsset> assetsList = new List<GameAsset>();
    public List<GameData> gameDataList = new List<GameData>();

    protected override void Initiate()
    {
        base.Initiate();

        for (int i = 0; i < gameDataList.Count; i++)
        {
            var data = gameDataList[i];
            data.LoadData();
            if (!data.HasData())
            {
                data.NewData();
            }
            else if (data.HasUpdateData())
            {
                data.UpdateData();
                Debug.LogErrorFormat("{0} has Update!!", data.GetName());
            }
            data.Initiate();
        }
    }

    public T GetAsset<T>() where T : ScriptableObject
    {
        try
        {
            return assetsList.Find(x => x.GetType().FullName == typeof(T).FullName) as T;
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("Missing ScriptableObject: {0}", typeof(T).FullName);
            return null;
        }
    }

    public T GetData<T>() where T : GameData
    {
        try
        {
            return gameDataList.Find(x => x.GetType().FullName == typeof(T).FullName) as T;
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("Missing GameData: {0}", typeof(T).FullName);
            return null;
        }
    }

    public void SaveData<T>(string key, T userSaveData)
    {
        string jsonDataEncode = JsonUtility.ToJson(userSaveData, false);
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(jsonDataEncode);
        string encodedText = Convert.ToBase64String(bytesToEncode);
        PlayerPrefs.SetString(key, encodedText);
        PlayerPrefs.Save();
    }

    public T LoadData<T>(string key)
    {
        string jsonData = PlayerPrefs.GetString(key);
        byte[] decodedBytes = Convert.FromBase64String(jsonData);
        string decodedText = Encoding.UTF8.GetString(decodedBytes);
        T _d = JsonUtility.FromJson<T>(decodedText);
        return _d;
    }

    public bool HasData(string key)
    {
        return PlayerPrefs.HasKey(key);
    }


    public void LoadAllData()
    {
        foreach (GameData data in gameDataList)
        {
            data.LoadData();
        }
    }


    public void SaveAllData()
    {
        foreach (GameData data in gameDataList)
        {
            data.SaveData();
        }
    }

    public void DeleteAllData()
    {
        foreach (GameData data in gameDataList)
        {
            data.NewData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveAllData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAllData();
        }
    }

    #region HELPERS

#if UNITY_EDITOR
    private void OnValidate()
    {
        assetsList.Clear();
        assetsList.AddRange(Resources.FindObjectsOfTypeAll<GameAsset>());

        gameDataList.Clear();
        GetComponents<GameData>(gameDataList);
    }
#endif

    #endregion

}
