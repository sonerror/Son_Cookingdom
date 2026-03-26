using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
namespace sonnv
{
    public class ItemNutInBoard : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] ParticleSystem[] pointPeels;
        [SerializeField] GameObject[] objHide;
        [SerializeField] Animation scaleAnim;
        [SerializeField] private AudioClip sfxTap;

        [SerializeField] private UnityEvent onDone;
        [SerializeField] private bool blockTap = false;
        int indexPeel = 0;
        private void Awake()
        {
            blockTap = false;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (blockTap == false)
            {
                SoundManager.PlaySFXOneShot(sfxTap);
                PlayCrackPeel();
                PlayCrackPeel();
                PlayCrackPeel();
            }
        }
        public void PlayCrackPeel()
        {
            if (indexPeel < pointPeels.Length)
            {
                pointPeels[indexPeel].gameObject.SetActive(true);
                pointPeels[indexPeel].Play();

                objHide[indexPeel].SetActive(false);
                scaleAnim.Stop();
                scaleAnim.Play();
                indexPeel++;
                if (indexPeel >= pointPeels.Length)
                {
                    blockTap = true;
                    foreach (ParticleSystem effect in pointPeels)
                    {
                        effect.gameObject.SetActive(false);
                    }
                    onDone?.Invoke();
                }
            }
        }
    }

}
