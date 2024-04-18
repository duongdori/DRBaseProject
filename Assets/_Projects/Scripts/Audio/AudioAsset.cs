using System;
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
    ADS,
    Hit,
    CLICK,
}

[CreateAssetMenu(menuName = "Asset/AudioAsset", fileName = "AudioAsset")]
public class AudioAsset : ScriptableObject
{
    [Serializable]
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
    public AudioData[] audioDataSet = Array.Empty<AudioData>();
    private Dictionary<AudioType, AudioClip> _dicClips = new();

    public void InitDic()
    {
        foreach (var item in audioDataSet)
        {
            if (!_dicClips.ContainsKey(item.audioType))
            {
                _dicClips.Add(item.audioType, item.audioClip);
            }
        }
    }

    public AudioClip GetClip(AudioType audioType)
    {
        if (_dicClips.TryGetValue(audioType, out var clip))
        {
            return clip;
        }
        
        Debug.LogErrorFormat("Missing audio type :{0}", audioType);
        return null;
    }
}
