using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace sonnv
{
    public class CookManager : Singleton<CookManager>
    {
        [Header("Step Add Cream")]
        [SerializeField] private SkeletonAnimation animCream;
        [SerializeField] private ParticleSystem particleCream;
        [SerializeField] private float timerDelayShowSmoke = 0.2f;
        [SerializeField] private SonSnapPoint snapPointFruit;

        public void OnStartStep()
        {
            StepAddCream();
        }
        private void StepAddCream()
        {
            if (animCream == null) return;
            animCream.gameObject.SetActive(true);
            StartCoroutine(IEShowParticle());
        }
        public void PlayAnimCream()
        {
            animCream.gameObject.SetActive(true);
        }
        private IEnumerator IEShowParticle()
        {
            yield return new WaitForSeconds(timerDelayShowSmoke);
            if (particleCream != null)
            {
                particleCream.gameObject.SetActive(true);
                particleCream.Play();
            }
            yield return new WaitForSeconds(1);
            snapPointFruit.ChangeCanSnap(true);
        }



    }
}