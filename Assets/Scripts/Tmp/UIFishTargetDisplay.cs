using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class UIFishTargetDisplay : MonoBehaviour
    {
        [SerializeField] UIFishTarget[] targetTypes;
        [SerializeField] Transform[] targetPos;
        [SerializeField] AudioClip sfxYay, sfxPop;

        int index = 0;
        public Vector3 GetPositon()
        {
            return targetPos[index++].position;
        }
        public void AddTarget(Fish.ColorType fishType)
        {
            //AudioManager.PlaySFX(sfxPop);
            targetTypes[(int)fishType - 1].AddThisTarget();
        }
        public bool CheckTarget(Fish.ColorType fishType)
        {

            for (int i = 0; i < targetTypes.Length; i++)
            {
                if (targetTypes[i].FishType == fishType
                    && targetTypes[i].IsActive
                    //&& !targetTypes[i].IsComplete
                    )
                {
                    targetTypes[i].OnCatched();

                    // AudioManager.PlaySFX(sfxYay);

                    if (IsComplete())
                    {
                        LevelFishing.Instance.OnGainPoint(10 + GetFishPoint(fishType));
                        LevelFishing.Instance.OnCompleteWave();
                    }
                    else
                    {
                        LevelFishing.Instance.OnGainPoint(GetFishPoint(fishType));
                    }
                    return true;
                }
            }
            return false;
        }
        public bool IsComplete()
        {
            for (int i = 0; i < targetTypes.Length; i++)
            {
                if (!targetTypes[i].IsComplete)
                    return false;
            }
            return true;
        }

        private int GetFishPoint(Fish.ColorType fishType)
        {
            switch (fishType)
            {
                case Fish.ColorType.Curver:
                    return 2;
                case Fish.ColorType.Speeder:
                    return 3;
                case Fish.ColorType.Wander:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}

