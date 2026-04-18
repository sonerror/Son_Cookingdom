using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEgg : SonMonoBehaviour
{
    [SerializeField] private AudioClip crackSfx;
    [SerializeField] private AudioClip kengSfx;
    [SerializeField] private Transform tfBowl;

    [UnityEngine.Scripting.Preserve]
    public void SfxCrack()
    {
        Debug.Log("====> LUNA ĐÃ ĐỌC ĐƯỢC CỜ SfxCrack! <====");

        if (crackSfx == null) return;
        SoundManager.PlaySFX(crackSfx);
    }

    [UnityEngine.Scripting.Preserve]
    public void SfxKeng()
    {
        Debug.Log("====> LUNA ĐÃ ĐỌC ĐƯỢC CỜ SfxKeng! <====");
        if (kengSfx == null) return;
        SoundManager.PlaySFX(kengSfx);
    }

    public void ShakeBowl()
    {
        Debug.Log("====> LUNA ĐÃ ĐỌC ĐƯỢC CỜ ShakeBowl! <====");

        if (tfBowl == null) return;
        tfBowl.DOShakePosition(0.2f, strength: new Vector3(0.1f, 0.1f, 0f), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
    }
}