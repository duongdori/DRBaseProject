using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    BG_MUSIC = 1,
    LOSE,
    WIN,
    BUTTON_CLICK,
    OBJECT_CLICK,
    CAMERA,
    FIREWORK,
    HPBD,
    LIGHT_CANDLE,
    ADS,
    Hit,
    CLICK,
}

[CreateAssetMenu(menuName = "Asset/AudioAsset", fileName = "AudioAsset")]
public class AudioAsset : ScriptableObject
{
    [System.Serializable]
    public struct AudioData
    {
        public AudioType audioType;
        public AudioClip audioClip;

        public AudioData(AudioType audioType, AudioClip audioClip)
        {
            this.audioType = audioType;
            this.audioClip = audioClip;
        }
    }
    public AudioData[] audioDataSet = new AudioData[0];
    private Dictionary<AudioType, AudioClip> dicClips = new Dictionary<AudioType, AudioClip>();

    public void InitDic()
    {
        foreach (var item in audioDataSet)
        {
            if (!dicClips.ContainsKey(item.audioType))
            {
                dicClips.Add(item.audioType, item.audioClip);
            }
        }
    }

    public AudioClip GetClip(AudioType audioType)
    {
        if (dicClips.ContainsKey(audioType))
        {
            return dicClips[audioType];
        }
        Debug.LogErrorFormat("Missing audio type :{0}", audioType);
        return null;
    }
}
