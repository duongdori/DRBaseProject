using System;
using UnityEngine;

[Serializable]
public class DataSaveUser
{
    public bool isNoAd;
    public DateTime startTime;
    public string startTimeString;

    public bool isVibrationOn;
    public bool isMusicOn;
    public bool isSoundOn;
    public bool isRateUs;
}

public class DataUser : GameData
{
    #region CONST
    #endregion

    #region EDITOR PARAMS
    [SerializeField] private DataSaveUser dataSave;
    #endregion

    #region PARAMS    
    #endregion

    #region PROPERTIES
    public DataSaveUser DataSave { get => dataSave; set => dataSave = value; }
    #endregion

    #region EVENTS
    #endregion

    #region METHODS
    public override void SaveData()
    {
        DataManager.Instance.SaveData<DataSaveUser>(GetName(), DataSave);
    }

    public override void LoadData()
    {
        DataSave = DataManager.Instance.LoadData<DataSaveUser>(GetName());
    }

    public override void NewData()
    {
        DataSave = new DataSaveUser();
        DataSave.isNoAd = false;
        DataSave.startTime = DateTime.Now;
        DataSave.startTimeString = DataSave.startTime.ToString();

        DataSave.isVibrationOn = true;
        DataSave.isMusicOn = true;
        DataSave.isSoundOn = true;
        DataSave.isRateUs = false;
    }
   
    public bool HasNoAd()
    {
        return DataSave.isNoAd;
    }

    public void SetNoAd(bool isNoAd)
    {
        DataSave.isNoAd = isNoAd;
        SaveData();
    }

    public bool HasMusic()
    {
        return DataSave.isMusicOn;
    }

    public void SetMusic(bool isOn)
    {
        DataSave.isMusicOn = isOn;
        SaveData();
    }

    public bool HasSound()
    {
        return DataSave.isSoundOn;
    }


    public void SetSound(bool isOn)
    {
        DataSave.isSoundOn = isOn;
        SaveData();
    }
    
    public bool HasRateUs()
    {
        return DataSave.isRateUs;
    }


    public void SetRateUs(bool isOn)
    {
        DataSave.isRateUs = isOn;
        SaveData();
    }

    public bool HasVibration()
    {
        return DataSave.isVibrationOn;
    }

    public void SetVibration(bool isOn)
    {
        DataSave.isVibrationOn = isOn;
        SaveData();
    }

    #endregion

    #region DEBUG
    #endregion
}
