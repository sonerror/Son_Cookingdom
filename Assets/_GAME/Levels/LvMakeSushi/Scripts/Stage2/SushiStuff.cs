using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class SushiStuff : MonoBehaviour
    {
        public SushiKit.SushiStuffType type;
        public Level127DraggableObject draggable;
        [SerializeField] private Transform tfTarget;
        [SerializeField] private SpriteRenderer render;
        public bool isEnable = true;

        public void OnTakeOff()
        {
            LevelMakeSushi.Instance.OnTakeOffStuff(type);
        }

        public void CheckTarget()
        {
            if(isEnable && Mathf.Abs(tfTarget.position.x - transform.position.x) < .75f
                && Mathf.Abs(tfTarget.position.y - transform.position.y) < 1.25f)
            {
                LevelMakeSushi.Instance.OnPutStuff(this);
            }
            else
            {
                LevelMakeSushi.Instance.OnNotUseStuff(type);
            }
        }
    }
}

