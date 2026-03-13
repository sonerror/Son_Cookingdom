using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class FlourMochiOrderStage2Controller : CharacterBase
    {
        private const string ANIMATION_POUND = "meo_gia__bot";
        private const string ANIMATION_KNEAD = "tho_nhoi_bot";

        [SerializeField] private MinigameMochiOrderStage2Controller minigameMochiOrderStage2Controller;

        public void ResetCharacter()
        {
            this.ResumeCurrentAnimation();
        }

        public override void OnPoundEnter()
        {
            base.OnPoundEnter();
            this.PlayAnimation(ANIMATION_POUND);
        }

        public override void OnPoundExecute()
        {
            base.OnPoundExecute();
        }

        public override void OnPoundExit()
        {
            base.OnPoundExit();
        }

        public override void OnKneadEnter()
        {
            base.OnKneadEnter();
            this.PlayAnimation(ANIMATION_KNEAD, false);
        }

        public override void OnKneadExecute()
        {
            base.OnKneadExecute();
        }

        public override void OnKneadExit()
        {
            base.OnKneadExit();
        }
    }
}

