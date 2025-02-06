using DG.Tweening;
using Satisgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class SushiPos : MonoBehaviour
    {
        [SerializeField] Transform[] tfPos;

        private int index = 0;

        private EmojiControl emoji => LevelMakeSushi.Instance.emojiStage2_dish;

        public void PutRollIn(SushiRoll roll)
        {
            roll.draggable.LockPosition();
            roll.transform.DOMove(tfPos[index].position, .1f);
            index++;

            if(index >=  tfPos.Length)
            {
                LevelMakeSushi.Instance.OnCompleteStage2();
            }
        }
        private void RemoveRoll(SushiRoll roll)
        {
            emoji.ShowNegative();
            roll.Despawn();
        }
    }
}

