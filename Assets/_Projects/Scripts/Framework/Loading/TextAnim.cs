using System.Collections;
using UnityEngine;
using TMPro;

public class TextAnim : MonoBehaviour
{
    [SerializeField] private string[] contents;
    [SerializeField] private float timeInterval;

    private TextMeshProUGUI _tmp;
    private int _id = 0;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(IE_Loop());
    }

    private IEnumerator IE_Loop()
    {
        while (true)
        {
            _tmp.text = contents[_id];
            _id++;
            if (_id == contents.Length) _id = 0;
            yield return new WaitForSeconds(timeInterval);
        }
    }
}
