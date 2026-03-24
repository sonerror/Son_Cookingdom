using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Link
{

    public class PlayVFX : MonoBehaviour
    {
        [SerializeField] ParticleSystem vfx;
        [SerializeField] AudioClip audioClip;

        public void Play()
        {
            vfx.Stop();
            vfx.Play();
            SoundManager.PlaySFXOneShot(audioClip, 1);
        }

        public void PlayLoop()
        {
            vfx.Stop();
            vfx.Play();
            SoundManager.PlaySFXOneShot(audioClip, 1);
        }

        public bool IsPlaying => vfx.isPlaying;

        public void Stop()
        {
            vfx.Stop();
            SoundManager.PlaySFXOneShot(audioClip, 1);
        }
    }
}
