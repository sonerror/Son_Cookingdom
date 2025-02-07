using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class SushiStuff : MonoBehaviour
    {
        public SushiKit.SushiStuffType type;
        public DraggableObject draggable;
        [SerializeField] private Transform tfTarget;
        [SerializeField] private SpriteRenderer render;
        public bool isEnable = true;

        public void OnTakeOff()
        {
            LevelMakeSushi.Ins.OnTakeOffStuff(type);
        }

        public void CheckTarget()
        {
            if (isEnable && Mathf.Abs(tfTarget.position.x - transform.position.x) < .75f
                && Mathf.Abs(tfTarget.position.y - transform.position.y) < 1.25f)
            {
                LevelMakeSushi.Ins.OnPutStuff(this);
            }
            else
            {
                LevelMakeSushi.Ins.OnNotUseStuff(type);
            }
        }
    }
}

