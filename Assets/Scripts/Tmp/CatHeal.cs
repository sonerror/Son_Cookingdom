using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnhPD.Fishing
{
    public class CatHeal : MonoBehaviour
    {
        [SerializeField] Image[] heartImages;
        [SerializeField] ParticleSystem fxHeal;
        [SerializeField] AudioClip sfxHeal, sfxHit;
        [SerializeField] Sprite heartLost;
        public int Hp { get; private set; }
        private void Start()
        {
            OnInit();
        }
        private void OnInit()
        {
            Hp = 3;
            for (int i = 0; i < heartImages.Length; i++)
            {
                heartImages[i].gameObject.SetActive(true);
            }
        }
        public void OnHit()
        {
            if (Hp > 0)
            {
                //AudioManager.PlaySFX(sfxHit);
                Hp--;
                heartImages[Hp].sprite = heartLost;
                if (Hp <= 0)
                {
                    // LevelFishing.Instance.LoseGame();
                    OnInit();
                }
            }
        }
        public void OnHeal()
        {
            fxHeal.Play();
            //AudioManager.PlaySFX(sfxHeal);
            if (Hp < 3)
            {
                heartImages[Hp].gameObject.SetActive(true);
                Hp++;
            }
        }
    }
}

