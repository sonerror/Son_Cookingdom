using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class CatMochiOrderStage2Controller : CharacterBase
    {
        private const string ANIMATION_POUND = "meo_gia_xoi";
        private const string ANIMATION_WAIT = "meo_wait";
        private const string ANIMATION_HIT = "meo_hit";
        private const string ANIMATION_SORRY = "meo_sorry";
        [SerializeField] private AudioSource audioSourcePound;
        [SerializeField] private AudioSource audioSourceHit;
        [SerializeField] private MinigameMochiOrderStage2Controller minigameMochiOrderStage2Controller;
        private void Start()
        {
            this.ChangeState(new PoundState());
        }

        public void PlaySfxPound()
        {
            audioSourcePound.Play();
        }

        public void ResetCharacter()
        {
            this.ChangeState(new PoundState());
        }

        #region Pound
        private float poundTimer = 0f;
        private float poundDuration;
        private bool isPlaySfxPound = false;
        private bool isRabbitKneadFlourCorrect = false;
        private bool isRabbitKneadFlourWrong = false;

        public void EventRabbitKneadFlourCorrect()
        {
            isRabbitKneadFlourCorrect = true;
        }

        public void EventRabbitKneadFlourWrong()
        {
            isRabbitKneadFlourWrong = true;
        }

        public override void OnPoundEnter()
        {
            minigameMochiOrderStage2Controller.GetFlourMochiOrderStage2Controller().ChangeState(new PoundState());
            base.OnPoundEnter();
            this.PlayAnimation(ANIMATION_POUND);
            poundTimer = 0f;
            isPlaySfxPound = false;
            poundDuration = this.GetDurationAnimation(ANIMATION_POUND);
        }

        public override void OnPoundExecute()
        {
            base.OnPoundExecute();

            poundTimer += Time.deltaTime;

            if (isRabbitKneadFlourCorrect && poundTimer >= 0.5f && poundTimer <= 0.6f)
            {
                Debug.Log("poundTimer: " + poundTimer);
                isRabbitKneadFlourCorrect = false;
                minigameMochiOrderStage2Controller.GetRabbitMochiOrderStage2Controller().ChangeState(new KneadState());
            }

            if (poundTimer >= poundDuration / 2 && !isPlaySfxPound)
            {
                PlaySfxPound();
                isPlaySfxPound = true;
            }

            if (poundTimer >= poundDuration)
            {
                // if (!minigameMochiOrderStage2Controller.GetFlourMochiOrderStage2Controller().CheckCurrentState(new PoundState()))
                // {
                //     minigameMochiOrderStage2Controller.GetFlourMochiOrderStage2Controller().ChangeState(new PoundState());
                // }

                minigameMochiOrderStage2Controller.GetFlourMochiOrderStage2Controller().ChangeState(new PoundState());

                if (isRabbitKneadFlourWrong)
                {
                    isRabbitKneadFlourWrong = false;
                    this.ChangeState(new HitState());
                }
                else
                {
                    this.ChangeState(new PoundState());
                }
            }
        }
        #endregion

        #region Wait
        public override void OnWaitEnter()
        {
            base.OnWaitEnter();
            this.PlayAnimation(ANIMATION_WAIT);
        }
        public override void OnWaitExecute()
        {
            base.OnWaitExecute();
        }
        public override void OnWaitExit()
        {
            base.OnWaitExit();
        }

        #endregion

        #region Hit
        private float hitTimer = 0f;
        private float hitDuration;
        private bool isPlaySfxHit = false;
        public override void OnHitEnter()
        {
            base.OnHitEnter();
            this.PlayAnimation(ANIMATION_HIT, false);
            minigameMochiOrderStage2Controller.GetRabbitMochiOrderStage2Controller().ChangeState(new HitState());
            hitTimer = 0f;
            hitDuration = this.GetDurationAnimation(ANIMATION_HIT);
            isPlaySfxHit = false;
        }
        public override void OnHitExecute()
        {
            base.OnHitExecute();
            hitTimer += Time.deltaTime;
            if (hitTimer >= hitDuration && !isPlaySfxHit)
            {
                audioSourceHit.Play();
                isPlaySfxHit = true;
                minigameMochiOrderStage2Controller.GetFlourMochiOrderStage2Controller().PauseCurrentAnimation();
                this.ChangeState(new SorryState());
            }
        }
        public override void OnHitExit()
        {
            base.OnHitExit();
        }
        #endregion

        #region Sorry
        public override void OnSorryEnter()
        {
            base.OnSorryEnter();
            this.PlayAnimation(ANIMATION_SORRY, true);
        }
        public override void OnSorryExecute()
        {
            base.OnSorryExecute();
        }

        public override void OnSorryExit()
        {
            base.OnSorryExit();
        }
        #endregion

        public override void OnPoundExit()
        {
            base.OnPoundExit();
        }
    }

}