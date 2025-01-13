using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class CrabTweezers : KingCrabTool
    {
        [SerializeField] Transform crabBody, pos;
        [SerializeField] List<SpriteRenderer> garbages;
        [SerializeField] SpriteRenderer egg;
        [SerializeField] SpriteRenderer topSprite;
        [SerializeField] AudioClip sfxGap;

        private bool isGap, isEgg;
        private SpriteRenderer currentGarbage;
        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);
            topSprite.enabled = true;
            topSprite.sortingOrder = spriteRenderer.sortingOrder;

            Tf.DOComplete();
            Tf.DORotate(new Vector3(0, 0, 15f), .3f);
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady) return;
            if (!isGap)
            {
                for (int i = 0; i < garbages.Count; i++)
                {
                    if (Vector2.Distance(pos.position, garbages[i].transform.position) < dropDistance)
                    {
                        //AudioManager.PlaySFX(sfxGap);
                        isGap = true;
                        currentGarbage = garbages[i];
                        currentGarbage.sortingOrder = spriteRenderer.sortingOrder;
                        garbages.Remove(garbages[i]);
                        currentGarbage.transform.SetParent(pos);
                        currentGarbage.transform.DOLocalMove(Vector2.zero, .1f);
                        return;
                    }
                }
            }
            else
            {
                if (Vector2.Distance(pos.position, crabBody.position) > dropDistance * 7)
                {
                    isGap = false;
                    SpriteRenderer gar = currentGarbage;
                    gar.transform.SetParent(null);
                    gar.transform.DOMoveY(gar.transform.position.y - 1f, .5f);
                    gar.sortingOrder = -19;
                    gar.DOFade(0, 1f).OnComplete(() =>
                    {
                        Destroy(gar.gameObject);
                    });

                    if (garbages.Count < 1)
                    {
                        if (!isEgg)
                        {
                            LevelKingCrab.Instance.OnCompleteRemoveIntestines();
                        }
                        else
                        {
                            LevelKingCrab.Instance.OnCompleteGapEgg();
                        }
                    }
                }
            }
        }
        protected override void Rewind(Action completeAction = null)
        {
            base.Rewind(completeAction);
            topSprite.enabled = false;
        }
        public void SetupForGapEgg()
        {
            IsReady = true;
            isEgg = true;
            garbages.Add(egg);
        }
    }
}

