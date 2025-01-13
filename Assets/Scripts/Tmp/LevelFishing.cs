using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class LevelFishing : MonoBehaviour
    {
        public static LevelFishing Instance;
        protected void Awake()
        {

            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        [SerializeField] TextMeshProUGUI txtWave;
        [SerializeField] GaintPointEffect pointEffect;
        // [SerializeField] WeatherController weatherController;
        // [SerializeField] SkyTimeController skyTimeController;
        [SerializeField] Transform canvasTarget;
        [SerializeField] FishingData data;

        [SerializeField] AudioClip sfxCompleteLevel;

        FishData[] fishTargets;
        ObstacleData[] obstacles;

        private int score;
        // private int levelIndex => data.levelIndex;
        public int LevelIndex { get; private set; }
        private int levelIndex => LevelIndex;

        public bool IsReady;

        private void LoadTarget()
        {

            StartCoroutine(delay());

            IEnumerator delay()
            {
                yield return new WaitForSeconds(.25f);
                for (int i = 0; i < fishTargets.Length; i++)
                {
                    int num = fishTargets[i].number;
                    for (int j = 0; j < num; j++)
                    {
                        targetDisplay.AddTarget(fishTargets[i].type);
                        yield return new WaitForSeconds(.25f);
                    }
                }
                IsReady = true;
            }
        }
        public void OnCompleteWave()
        {
            // AudioManager.PlaySFX(sfxCompleteLevel);
            data.levelIndex++;
        }



        public void OnGainPoint(int point)
        {
            pointEffect.DisplayPoint(point);

            score += point;
            // SetScore(score);
        }
        [SerializeField] UIFishTargetDisplay targetDisplay;
        public bool CheckFish(Fish.ColorType type)
        {
            return targetDisplay.CheckTarget(type);
        }
        public bool IsComplete => targetDisplay.IsComplete();
    }
}

