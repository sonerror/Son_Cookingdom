using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class CapybaraFishing : MonoBehaviour
    {
        #region config anim name
        private const string ANIM_FISHING_START = "GAME-ACTION/1-fishing-start";
        private const string ANIM_FISHING_IDLE = "GAME-ACTION/2-fishing-idle";
        private const string ANIM_FISHING_CATCHING_LOOP = "GAME-ACTION/4-fishing-catching-loop";
        private const string ANIM_FISHING_CATCH = "GAME-ACTION/5-fishing-catch";
        private const string ANIM_FISHING_CATCH_REEL_LOOP = "GAME-ACTION/6-fishing-catch-reel-loop";
        private const string ANIM_FISHING_CATCH_FINISH = "GAME-ACTION/7-fishing-catch-finish";
        private const string ANIM_SAD = "GAME-ACTION/idle-sit-fail";
        private const string ANIM_ANGRY = "GAME-ACTION/idle-sit-fail2";
        private const string ANIM_HAPPY = "GAME-ACTION/idle-sit-cheer";
        #endregion
        [SerializeField] SkeletonAnimation anim;
        [SerializeField] Transform board;
        [SerializeField] Level110Hook hook;

        [SerializeField] AudioClip sfxSad, sfxAngry, sfxHappy;
        TrackEntry trackCapy, trackEmoji;
        private void Start()
        {
            hook.ChangeState(Level110Hook.HookState.None);

            anim.state.SetAnimation(0, ANIM_FISHING_IDLE, true);

            board.localPosition = new Vector2(5, 0);
            board.DOLocalMoveX(.85f, 1f).OnComplete(() =>
            {
                GetReady();
            });
        }
        public void GetReady()
        {
            OnStartFishing(() =>
            {
                hook.gameObject.SetActive(true);
                hook.OnInit();
            });
        }
        public void OnDamaged()
        {
            //AudioManager.PlaySFX(sfxSad);
            trackCapy = anim.state.SetAnimation(0, ANIM_SAD, false);
            trackCapy.Complete += entry =>
            {
                GetReady();
            };
        }
        public void OnAngry()
        {
            //AudioManager.PlaySFX(sfxAngry );
            trackEmoji = anim.state.SetAnimation(1, ANIM_ANGRY, false);
            trackEmoji.Complete += entry =>
            {
                anim.state.ClearTrack(1);
                GetReady();
            };
        }
        public void OnHappy()
        {
            // AudioManager.PlaySFX(sfxHappy);
            trackEmoji = anim.state.SetAnimation(1, ANIM_HAPPY, false);
            trackEmoji.Complete += entry =>
            {
                anim.state.ClearTrack(1);
                GetReady();
            };
        }
        public void OnStartFishing(Action action)
        {
            trackCapy = anim.state.SetAnimation(0, ANIM_FISHING_START, false);
            trackCapy.Complete += entry =>
            {
                CatchingLoop(entry);
                action?.Invoke();
            };
        }
        public void OnCatched()
        {
            trackCapy = anim.state.SetAnimation(0, ANIM_FISHING_CATCH, false);
            trackCapy.Complete += CatchReelLoop;
        }
        public void OnFishReturned(Action action)
        {
            anim.state.ClearTrack(0);
            trackCapy = anim.state.SetAnimation(0, ANIM_FISHING_CATCH_FINISH, false);
            trackCapy.Complete += entry =>
            {
                action?.Invoke();
            };
        }

        #region anim state
        private void Idle(TrackEntry trackEntry)
        {
            trackCapy = anim.state.SetAnimation(0, ANIM_FISHING_IDLE, true);
        }
        private void CatchingLoop(TrackEntry trackEntry)
        {
            trackCapy = anim.state.SetAnimation(0, ANIM_FISHING_CATCHING_LOOP, true);
        }
        private void CatchReelLoop(TrackEntry trackEntry)
        {
            trackCapy = anim.state.SetAnimation(0, ANIM_FISHING_CATCH_REEL_LOOP, true);
        }
        #endregion
    }
}

