using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;



using UnityEngine;
namespace sonnv
{
    public class Level628 : NextStepLevel
    {
        [SerializeField] private AudioClip pickSfx;
        [Header("ZoomSettings")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minZoom = 2f;
        [SerializeField] private float maxZoom = 10f;
        [SerializeField] private float zoomDuration = 0.25f;
        private Tween currentTween;

        [SerializeField] private Phase1Donut phase1;
        public static Level628 Ins;


        protected override void Awake()
        {
            base.Awake();

            if (Ins == null)
                Ins = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void OnEndStep(bool playSound = false)
        {
            DoneStep(true, playSound);
            TryNextStep();
        }
        public void FinishStep()
        {
            DoneStep();
        }

        public void SmoothZoomBy(float deltaSize, float zoomDuration)
        {
            float targetSize = Camera.orthographicSize + deltaSize;
            targetSize = Mathf.Clamp(targetSize, minZoom, maxZoom);

            if (Mathf.Approximately(targetSize, Camera.orthographicSize)) return;

            currentTween?.Kill();
            currentTween = Camera.DOOrthoSize(targetSize, zoomDuration).SetEase(Ease.OutCubic);
        }

        public void SmoothZoomTo(float targetSize)
        {
            targetSize = Mathf.Clamp(targetSize, minZoom, maxZoom);
            if (Mathf.Approximately(targetSize, Camera.orthographicSize)) return;
            currentTween?.Kill();
            currentTween = Camera.DOOrthoSize(targetSize, zoomDuration).SetEase(Ease.OutCubic);
        }
        public void SmoothZoomTo(float targetSize, float zoomDuration)
        {
            targetSize = Mathf.Clamp(targetSize, minZoom, maxZoom);
            if (Mathf.Approximately(targetSize, Camera.orthographicSize)) return;
            currentTween?.Kill();
            currentTween = Camera.DOOrthoSize(targetSize, zoomDuration).SetEase(Ease.OutCubic);
        }
        public void PlayPickSfx()
        {
            SoundManager.Ins.PlayFx(FxType.Click);
        }

        protected override void InitStepActions()
        {
            //Phase 1
            // AddStepAction(1, phase1.OnCallStep1);
            // AddStepAction(2, phase1.OnCallStep2);
            // AddStepAction(3, phase1.OnCallStep3);
            // AddStepAction(4, phase1.OnCallStep4);
            // AddStepAction(5, phase1.OnCallStep5);
            // AddStepAction(6, phase1.OnCallStep6);

        }
        protected override void Start()
        {
            base.Start();
            phase1.OnCallStep0();
        }
        /*protected override void InitStepActions()
        {
            //Phase 2
            AddStepAction(1, phase2.OnCallStep8);
            AddStepAction(2, phase2.OnCallStep9);
            AddStepAction(3, phase2.OnCallStep10);
            AddStepAction(4, phase2.OnCallStep11);
            AddStepAction(5, phase2.OnCallStep12);
            AddStepAction(6, phase2.OnCallStep13);
            AddStepAction(7, phase2.OnCallStep14);
            AddStepAction(8, phase2.OnCallStep15);
            AddStepAction(28, OnEndGame);
        }
        protected override void Start()
        {
            base.Start();
            phase2.OnCallStep7();
        }*/
        /*protected override void InitStepActions()
        {
            //Phase 4
            AddStepAction(1, phase4.OnCallStep23);
            AddStepAction(2, phase4.OnCallStep24);
            AddStepAction(3, phase4.OnCallStep25);
            AddStepAction(4, phase4.OnCallStep26);
            AddStepAction(5, phase4.OnCallStep27);

            AddStepAction(28, OnEndGame);
        }
        protected override void Start()
        {
            base.Start();
            phase4.OnCallStep22();
        }
*/

        /*protected override void InitStepActions()
        {
            //Phase 3
            AddStepAction(1, phase3.StartStep17);
            AddStepAction(2, phase3.OnCallStep18);
            AddStepAction(3, phase3.OnCallStep19);
            AddStepAction(4, phase3.OnCallStep20);
            AddStepAction(5, OnEndGame);
        }
        protected override void Start()
        {
            base.Start();
            phase3.OnCallStep16();
        }*/


        [SerializeField] private Transform tfBotOtherPhase4;

        private Tween moveTween;
        public void MoveAndZoomTo(Transform target, float targetZoom, System.Action onComplete)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, Camera.transform.position.z);

            moveTween?.Kill();

            currentTween?.Kill();

            Sequence seq = DOTween.Sequence();

            seq.Append(Camera.transform.DOMove(targetPos, 1.5f).SetEase(Ease.InOutSine));
            seq.Append(tfBotOtherPhase4.DOMoveY(-50f, 0.5f));
            seq.Join(Camera.DOOrthoSize(targetZoom, 1.5f).SetEase(Ease.OutCubic));
            seq.OnComplete(() => onComplete?.Invoke());
        }
    }
}