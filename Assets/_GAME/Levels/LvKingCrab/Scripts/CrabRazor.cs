using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class CrabRazor : KingCrabTool
    {
        [SerializeField] Transform left, right, lid;
        [SerializeField] GameObject painter;
        [SerializeField] Transform meatDump, meatDump2;
        bool isLid;
        float rateL, rateR, rateM;
        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);
            Tf.DOComplete();
            Tf.DORotate(new Vector3(0, 0, 15f), .3f);

            if (IsReady)
            {
                painter.SetActive(true);
                LevelKingCrab.Instance.StartPainter();
            }
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady) return;
            if (!isLid)
            {
                if (Vector2.Distance(painter.transform.position, left.transform.position) < dropDistance)
                {
                    rateL += Time.deltaTime;
                    rateL = Mathf.Clamp01(rateL);
                }
                if (Vector2.Distance(painter.transform.position, right.transform.position) < dropDistance)
                {
                    rateR += Time.deltaTime;
                    rateR = Mathf.Clamp01(rateR);
                }

                float rate = (rateL + rateR) / 2;
                meatDump.localScale = Vector3.one * 1.5f * rate;
            }
            else
            {
                if (Vector2.Distance(painter.transform.position, lid.position) < dropDistance * 2)
                {
                    rateM += Time.deltaTime;
                    rateM = Mathf.Clamp01(rateM);

                    meatDump2.localScale = Vector3.one * 1f * rateM;
                }
            }

        }
        protected override void MouseUp(BaseEventData eventData)
        {
            base.MouseUp(eventData);
            painter.SetActive(false);
            LevelKingCrab.Instance.EndPainter();
        }
        public void SetupForLid()
        {
            IsReady = true;
            isLid = true;
        }
    }
}

