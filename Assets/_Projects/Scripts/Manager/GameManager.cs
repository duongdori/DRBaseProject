using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public LevelController levelController;

    public void OnInit()
    {
        levelController.OnInit();
    }
    
    private void LoadLevel(int level)
    {
        levelController.OnLevelLoad(level);
    }

    public void StartLevel()
    {
        levelController.OnLevelStart();
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(DataManager.Instance.GetData<DataLevel>().CurrentLevelId);
    }
    
    public void BackToHome()
    {
        UIManager.Instance.HideScreen<ScreenInGame>();
        UIManager.Instance.ShowScreen<ScreenHome>();
    }
    
    public void OnWin()
    {
        DataManager.Instance.GetData<DataLevel>().PassLevel();
        UIManager.Instance.ShowPopup<PopupWin>();
    }
   
    public void OnLoss()
    {
        UIManager.Instance.ShowPopup<PopupLose>();
    }
}
