using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class AnimEgg : SonMonoBehaviour
    {
        [SerializeField] private Transform tfBowl;
        public void SfxCrack()
        {

        }
        public void SfxKeng()
        {

        }
        public void ShakeBowl()
        {
            if (tfBowl == null) return;
            tfBowl.DOShakePosition(0.2f, strength: new Vector3(0.1f, 0.1f, 0f), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
        }
    }

}
