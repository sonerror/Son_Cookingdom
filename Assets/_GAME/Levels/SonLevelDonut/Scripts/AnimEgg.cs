using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class AnimEgg : SonMonoBehaviour
    {
        [SerializeField] private AudioClip crackSfx;
        [SerializeField] private AudioClip kengSfx;
        [SerializeField] private Transform tfBowl;
        public void SfxCrack()
        {
            if (crackSfx == null) return;
            SoundManager.PlaySFX(crackSfx);
        }
        public void SfxKeng()
        {
            if (kengSfx == null) return;
            SoundManager.PlaySFX(kengSfx);
        }
        public void ShakeBowl()
        {
            if (tfBowl == null) return;
            tfBowl.DOShakePosition(0.2f, strength: new Vector3(0.1f, 0.1f, 0f), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
        }
    }

}
