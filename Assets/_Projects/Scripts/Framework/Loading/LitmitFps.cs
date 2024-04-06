using UnityEngine;

public class LitmitFps : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}
