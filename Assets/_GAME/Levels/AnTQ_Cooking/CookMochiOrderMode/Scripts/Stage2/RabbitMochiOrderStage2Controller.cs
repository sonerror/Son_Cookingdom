using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class RabbitMochiOrderStage2Controller : CharacterBase
    {
        private const string ANIMATION_KNEAD = "tho_nhoi_xoi";
        private const string ANIMATION_WAIT = "tho_wait";
        private const string ANIMATION_HIT = "tho_hurt1";
        private const string ANIMATION_HURT = "tho_hurt2";
        [SerializeField] private AudioSource audioSourceKnead;
        [SerializeField] private AudioSource audioSourceHit;
        [SerializeField] private AudioSource audioSourceHurt;
        [SerializeField] private MinigameMochiOrderStage2Controller minigameMochiOrderStage2Controller;
        private void Start()
        {
            this.ChangeState(new WaitState());
        }

        public override void Update()
        {
            base.Update();
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     this.ChangeState(new KneadState());
            // }
        }

        public void ResetCharacter()
        {
            this.ChangeState(new WaitState());
        }

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

        #region Knead 
        private float kneadTimer = 0f;
        private float kneadDuration;
        private bool isPlaySfxKnead = false;

        public override void OnKneadEnter()
        {
            base.OnKneadEnter();
            this.PlayAnimation(ANIMATION_KNEAD, false);
            kneadTimer = 0f;
            kneadDuration = this.GetDurationAnimation(ANIMATION_KNEAD);
            minigameMochiOrderStage2Controller.GetFlourMochiOrderStage2Controller().ChangeState(new KneadState());
            isPlaySfxKnead = false;
        }
        public override void OnKneadExecute()
        {
            base.OnKneadExecute();
            kneadTimer += Time.deltaTime;
            if (kneadTimer >= kneadDuration / 2 && !isPlaySfxKnead)
            {
                audioSourceKnead.Play();
                isPlaySfxKnead = true;
            }
            if (kneadTimer >= kneadDuration)
            {
                isPlaySfxKnead = false;
                this.ChangeState(new WaitState());
            }
        }
        public override void OnKneadExit()
        {
            base.OnKneadExit();
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
                this.ChangeState(new HurtState());
            }
        }
        public override void OnHitExit()
        {
            base.OnHitExit();
        }
        #endregion

        #region Hurt
        private float hurtTimer = 0f;
        private float hurtDuration;
        private bool isPlaySfxHurt = false;


        public override void OnHurtEnter()
        {
            base.OnHurtEnter();
            this.PlayAnimation(ANIMATION_HURT, true);
            hurtTimer = 0f;
            hurtDuration = this.GetDurationAnimation(ANIMATION_HURT);
            isPlaySfxHurt = false;
        }
        public override void OnHurtExecute()
        {
            base.OnHurtExecute();
            hurtTimer += Time.deltaTime;
            if (hurtTimer >= hurtDuration * 0.5f && !isPlaySfxHurt)
            {
                audioSourceHurt.Play();
                isPlaySfxHurt = true;
            }
        }

        public override void OnHurtExit()
        {
            base.OnHurtExit();
        }
        #endregion
    }
}

