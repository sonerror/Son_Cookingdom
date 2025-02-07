using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FxType
{
    Click = 0,
    WaterDrop = 1,
    SfxEmoijPositive = 2,
    SfxEmoijNegative = 3,
    SfxDop4 = 4,
    SfxDop5 = 5,
    WaterBoiling = 6,
    SfxDop2 = 7,
    SfxPeel = 8,









    // =================
    Pick = 70,
    Drop = 71,
    Cut = 72,
    Hammer = 73,
    OpenShell = 74,
    Push = 75,
    Gap = 76,
    PaintBush = 77,
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip[] audioClips;
    public AudioSource bgm;
    private AudioSource[] fx = new AudioSource[11];

    bool isMute = false;

    public void PlayFx(FxType fxType)
    {
        if (!isMute)
        {
            if (fx[(int)fxType] == null)
            {
                fx[(int)fxType] = new GameObject().AddComponent<AudioSource>();
                fx[(int)fxType].clip = audioClips[(int)fxType];
            }

            fx[(int)fxType].Play();
        }
    }

    public void PlaySoundLoop(FxType fxType)
    {
        if (!isMute)
        {
            if (fx[(int)fxType] == null)
            {
                fx[(int)fxType] = new GameObject().AddComponent<AudioSource>();
                fx[(int)fxType].clip = audioClips[(int)fxType];
            }

            fx[(int)fxType].loop = true;
            fx[(int)fxType].Play();
        }
    }

    public void StopSoundLoop(FxType fxType)
    {
        if (fx[(int)fxType] != null)
        {
            fx[(int)fxType].loop = false;
            fx[(int)fxType].Stop();
        }
    }

    public IEnumerator IE_PlayFxAfterTime(FxType fxType, float time)
    {
        yield return Cache.GetWFS(time);
        if (!isMute)
        {
            if (fx[(int)fxType] == null)
            {
                fx[(int)fxType] = new GameObject().AddComponent<AudioSource>();
                fx[(int)fxType].clip = audioClips[(int)fxType];
            }

            fx[(int)fxType].Play();
        }
    }

    public void PlayFxAfterTime(FxType fxType, float time)
    {
        StartCoroutine(IE_PlayFxAfterTime(fxType, time));
    }

    public void Mute()
    {
        bgm.Stop();
        for (int i = 0; i < fx.Length; i++)
        {
            if (fx[i] != null)
            {
                fx[i].Stop();
            }
        }
    }
}