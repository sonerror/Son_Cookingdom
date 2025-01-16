using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class Scissors : KingCrabTool
    {
        [SerializeField] Transform pos;
        [SerializeField] Sprite close, open;
        [SerializeField] FxType sfx = FxType.Cut;
        [SerializeField] List<CrabLeg> legs;
        [SerializeField] List<CrabLegPart> parts;
        [SerializeField] CrabBody crabBody;
        [SerializeField] SpriteRenderer crabLungL, crabLungR;

        public ItemDrop meatRight, meatLeft;
        enum State
        {
            CutCrab = 0,
            CutLeg = 1,
            CutBody = 2,
            CutLung = 3,
        }
        private bool isCooldown;
        private State state;

        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);
            isCooldown = false;
            spriteRenderer.sprite = open;

            Tf.DOComplete();
            Tf.DORotate(new Vector3(0, 0, 20f), 0.3f);
            if (IsReady) TutorialManager.Ins.MouseDownItem();
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady || isCooldown) return;
            switch (state)
            {
                case State.CutCrab:
                    for (int i = 0; i < legs.Count; i++)
                    {
                        if (Vector2.Distance(pos.position, legs[i].transform.position) < dropDistance)
                        {
                            legs[i].OnCut();
                            legs.Remove(legs[i]);

                            StartCoroutine(Cooldown());

                            if (legs.Count < 1)
                            {
                                LevelKingCrab.Instance.OnCompleteCutCrab();
                            }
                        }
                    }
                    break;
                case State.CutLeg:
                    for (int i = 0; i < parts.Count; i++)
                    {
                        if (isCooldown) return;
                        if (Vector2.Distance(pos.position, parts[i].transform.position) < dropDistance)
                        {
                            parts[i].OnCut();
                            parts.Remove(parts[i]);

                            StartCoroutine(Cooldown());

                            if (parts.Count < 1)
                            {
                                LevelKingCrab.Instance.OnCompleteCutLeg();
                            }
                        }
                    }
                    break;
                case State.CutBody:
                    if (Vector2.Distance(pos.position, crabBody.transform.position) < dropDistance)
                    {
                        crabBody.OnCut();
                        LevelKingCrab.Instance.OnCompleteCutCrabBody();
                        StartCoroutine(Cooldown());

                    }
                    break;
                case State.CutLung:
                    if (crabLungL == null && crabLungR == null)
                    {
                        LevelKingCrab.Instance.OnCompleteCutLung();
                        return;
                    }
                    if (crabLungL != null && Vector2.Distance(pos.position, crabLungL.transform.position) < dropDistance)
                    {
                        crabBody.OnCutLungLeft();
                        StartCoroutine(Cooldown());
                        meatLeft.isDrop = true;
                        crabLungL = null;
                    }
                    if (crabLungR != null && Vector2.Distance(pos.position, crabLungR.transform.position) < dropDistance)
                    {
                        crabBody.OnCutLungRight();
                        StartCoroutine(Cooldown());
                        meatRight.isDrop = true;
                        crabLungR = null;
                    }
                    break;
            }

        }



        protected override void Rewind(Action completeAction = null)
        {
            base.Rewind(completeAction);
            spriteRenderer.sprite = close;
        }

        private IEnumerator Cooldown()
        {
            isCooldown = true;
            spriteRenderer.sprite = close;
            SoundManager.Ins.PlayFx(sfx);

            yield return new WaitForSeconds(.2f);
            isCooldown = false;
            spriteRenderer.sprite = open;
        }
        public void SetupForCutLeg()
        {
            state = State.CutLeg;
            IsReady = true;
        }
        public void SetupForCutCrab()
        {
            state = State.CutBody;
            IsReady = true;
        }
        public void SetupForCutLung()
        {
            state = State.CutLung;
            IsReady = true;
        }
        public override void OnComplete()
        {
            base.OnComplete();
            StopAllCoroutines();
        }
    }
}

