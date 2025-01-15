using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FxType
{
    Pick = 0,
    Drop = 1,
    Cut = 2,
    Hammer = 3,
    OpenShell = 4,
    Push = 5,
    Gap = 6,
    PaintBush = 7,
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip[] audioClips;
    public AudioSource sound1;
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
        sound1.Stop();
        for (int i = 0; i < fx.Length; i++)
        {
            if (fx[i] != null)
            {
                fx[i].Stop();
            }
        }
    }
}