using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class CrabSpoon : KingCrabTool
    {
        [SerializeField] SpriteRenderer crabSauce;
        [SerializeField] Transform crabBody;
        [SerializeField] CrabBowl bowl;

        private bool isHaveSauce, isCompleteSauce;

        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);

            crabSauce.sortingOrder = spriteRenderer.sortingOrder;

            Tf.DOComplete();
            Tf.DORotate(new Vector3(0, 0, 15f), .3f);

            if (IsReady)
            {
                LevelKingCrab.Instance.StartPainter();
            }
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady) return;
            if (!isCompleteSauce)
            {
                if (!isHaveSauce && Vector2.Distance(crabSauce.transform.position, crabBody.position) < dropDistance)
                {
                    isHaveSauce = true;
                    crabSauce.DOFade(1, 1f).OnComplete(() =>
                    {
                        isCompleteSauce = true;
                    });
                    crabBody.GetComponentInChildren<SpriteRenderer>().DOFade(0, 1f);
                }
            }
            else
            {
                Debug.Log("Drop sauce" + (Vector2.Distance(crabSauce.transform.position, bowl.transform.position) < dropDistance));
                if (Vector2.Distance(crabSauce.transform.position, bowl.transform.position) < dropDistance)
                {
                    Debug.Log("Drop sauce1111");
                    bowl.OnSauceIn();
                    crabSauce.gameObject.SetActive(false);
                    LevelKingCrab.Instance.OnPutSauceInBowl();
                }
            }
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            base.MouseUp(eventData);
            LevelKingCrab.Instance.EndPainter();
        }
        public void OnCompleteSauce()
        {
            LevelKingCrab.Instance.EndPainter();

            isCompleteSauce = true;
        }
    }
}

