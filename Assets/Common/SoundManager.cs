using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SoundType
{
    OneShotSFX,
    LoopSFX,
    Music
}

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundTypeInfo
    {
        public SoundType _SoundType;
        public AudioSource _AudioSource;
    }

    [SerializeField] private SoundTypeInfo[] m_SoundTypeInfo;
    private static SoundManager mInstance;
    // Start is called before the first frame update
    void Awake()
    {
        mInstance = this;
    }

    public static void Play(AudioClip clip, SoundType soundType)
    {
        SoundTypeInfo soundTypeInfo = System.Array.Find(mInstance.m_SoundTypeInfo, x => x._SoundType == soundType);
        if (soundType == SoundType.OneShotSFX)
            soundTypeInfo._AudioSource.PlayOneShot(clip);
        else
        {
            soundTypeInfo._AudioSource.clip = clip;
            soundTypeInfo._AudioSource.Play();
        }
    }
}
