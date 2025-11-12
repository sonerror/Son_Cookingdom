using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FxType
{
  Xe = 0,
  Do = 1,
  Pickup1 = 2,
  Place1 = 3,
  Pickup2 = 4,
  Pickup3 = 5,
  Soup = 6,
  OvenClose = 7,
  OvenDone = 8,
  OvenRunning = 9,
  Fire = 10,

  EmojiPositive = 11,
  EmojiNegative = 12,
  None = 20,
}

public class SoundManager : Singleton<SoundManager>
{
  public AudioClip[] audioClips;
  public AudioSource bgm;
  private AudioSource[] fx = new AudioSource[13];

  bool isMute = false;

  public void PlayFx(FxType fxType)
  {
    if (fxType == FxType.None) return;
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
        fx[(int)fxType].playOnAwake = false;
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
    if (fxType == FxType.None) return;
    StartCoroutine(IE_PlayFxAfterTime(fxType, time));
  }

  public void Mute()
  {
    bgm.Stop();
    isMute = true;
    for (int i = 0; i < fx.Length; i++)
    {
      if (fx[i] != null)
      {
        fx[i].Stop();
      }
    }
  }
}