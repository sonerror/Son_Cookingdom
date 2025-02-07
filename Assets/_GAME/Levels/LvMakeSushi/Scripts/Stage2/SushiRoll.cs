using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AnhPD.MakeSushi
{
    public class SushiRoll : MonoBehaviour
    {
        public SushiKit.SushiStuffType type;
        [SerializeField] GameObject fish, avocado;
        public DraggableObject draggable;

        private SushiPos sushiPos => LevelMakeSushi.Ins.sushiPos;

        public void CheckDistance()
        {
            if (Vector2.Distance(sushiPos.transform.position, transform.position) < .5f)
            {
                sushiPos.PutRollIn(this);
            }
        }

        public void OnInit(SushiKit.SushiStuffType type)
        {
            this.type = type;
            if (type == SushiKit.SushiStuffType.Fish)
            {
                fish.SetActive(true);
                avocado.SetActive(false);
            }
            else
            {
                fish.SetActive(false);
                avocado.SetActive(true);
            }
            draggable.UnlockPosition();
        }
        public void Despawn()
        {
            draggable.LockPosition();
            transform.DOLocalMoveX(-5f, .5f).OnComplete(() =>
            {
                ObjectPoolDictArray.Ins.ReleaseGameObject(gameObject);
            });
        }
    }
}

