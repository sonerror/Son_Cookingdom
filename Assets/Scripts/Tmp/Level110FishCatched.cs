using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class Level110FishCatched : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private GameObject[] fishes;
        [SerializeField] private GameObject[] garbages;

        private Fish.ColorType fishColor;
        public Fish.ColorType FishColor => fishColor;

        public void OnInit()
        {
            DeActiveAllObject();
            fishColor = Fish.ColorType.None;
            DisableObject();
        }

        public void OnInitFishSprite(Fish.ColorType type)
        {
            if (anim != null)
            {
                anim.enabled = true;
            }
            fishColor = type;
            DeActiveAllObject();

            int index = (int)type;
            if (index > 0 && index <= fishes.Length)
            {
                fishes[index - 1].SetActive(true);
            }
        }
        public void OnInitGarbageSprite(Garbage.GType gType)
        {
            if (anim != null)
            {
                anim.enabled = false;
            }

            fishColor = Fish.ColorType.None;
            DeActiveAllObject();

            int index = (int)gType;
            if (index >= 0 && index < garbages.Length)
            {
                garbages[index].SetActive(true);
            }
        }
        private void DeActiveAllObject()
        {
            for (int i = 0; i < fishes.Length; i++)
            {
                fishes[i].SetActive(false);
            }
            for (int i = 0; i < garbages.Length; i++)
            {
                garbages[i].SetActive(false);
            }
        }
        public void EnableObject()
        {
            gameObject.SetActive(true);
        }
        public void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}

