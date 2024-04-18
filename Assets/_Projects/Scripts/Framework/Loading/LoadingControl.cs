using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float loadingTime = 6f;

    private void Start()
    {
        LoadScene(1);
    }

    public void LoadScene(int sceneID)
    {
        loadingBar.fillAmount = 0f;
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    private IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneID);
        loadOperation.allowSceneActivation = false;

        float startTime = Time.time;
        float targetTime = startTime + loadingTime;

        while (!loadOperation.isDone || Time.time < targetTime)
        {
            float progressValue = Mathf.Clamp01((Time.time - startTime) / loadingTime);
            loadingBar.fillAmount = progressValue;
            loadingText.SetText(Mathf.FloorToInt(progressValue * 100) + "%");

            if (progressValue >= 1f)
            {
                loadingBar.fillAmount = 1f;
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    
    #region Old Code

    // public float loadingDuration;
    //
    // public Transform tfBulletContainer;
    //
    // public List<Transform> tfBullets;
    //
    // private void Start()
    // {
    //     LoadScene("Home", loadingDuration);
    // }
    //
    // public void LoadScene(string id, float time)
    // {
    //     StartCoroutine(IELoadScene(id, time));
    // }
    //
    // private AsyncOperation _async;
    //
    // // float timeLoadAds = 0;
    //
    //
    // private IEnumerator IELoadScene(string id, float time)
    // {
    //     float duration = time / 12f;
    //
    //     StartCoroutine(RotateContainer(duration, 0));
    //
    //     yield return new WaitForSeconds(0.1f);
    //     _async = SceneManager.LoadSceneAsync(id);
    //     _async.allowSceneActivation = false;
    //
    //     yield return new WaitUntil(() => _async.progress == 0.9f);
    //     yield return new WaitForSeconds(time);
    //
    //     SceneManager.LoadScene(id);
    // }
    //
    // private IEnumerator RotateContainer(float time, int index)
    // {
    //     if (index >= tfBullets.Count)
    //     {
    //         Debug.Log("Het roi");
    //         yield break;
    //     }
    //
    //     Vector3 vectorRotate = new Vector3(tfBulletContainer.localEulerAngles.x , tfBulletContainer.localEulerAngles.y, tfBulletContainer.localEulerAngles.z -60f);
    //     Vector3 vectorRotateMax = new Vector3(vectorRotate.x, vectorRotate.y, vectorRotate.z - 10f);
    //     Vector3 vectorRotateMin = new Vector3(vectorRotate.x, vectorRotate.y, vectorRotate.z + 10f);
    //     tfBulletContainer.DOLocalRotate(vectorRotateMax, time).OnComplete(() =>
    //     {
    //         tfBulletContainer.DOLocalRotate(vectorRotateMin, time / 6).OnComplete(() =>
    //         {
    //             tfBulletContainer.DOLocalRotate(vectorRotate, time / 6).OnComplete(() =>
    //             {
    //                 DOVirtual.DelayedCall(0.2f, () =>
    //                 {
    //                     tfBullets[index].gameObject.SetActive(true);
    //                     StartCoroutine(RotateContainer(time, ++index));
    //                 });
    //             });
    //         });
    //     });
    // }

    #endregion
}
