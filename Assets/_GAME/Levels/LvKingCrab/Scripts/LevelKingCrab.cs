using AnhPD.Fishing;
using DG.Tweening;
using Satisgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.KingCrab
{
    public class LevelKingCrab : LevelBase
    {
        public static LevelKingCrab Instance;

        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public bool IsReady;
        protected override void Start()
        {
            base.Start();
            scissors.IsReady = true;
            _hint = hints[1];
        }

        public EmojiControl emoji;
        [SerializeField] Sprite[] hints;
        [SerializeField] Scissors scissors;
        [SerializeField] Transform dish, board, tool;
        [SerializeField] CrabStickTool stick;
        [SerializeField] CrabBody crabBody;
        [SerializeField] CrabHammer hammer;
        [SerializeField] Transform legs;
        [SerializeField] CrabPry pry;
        [SerializeField] CrabTweezers tweezers;
        [SerializeField] CrabSpoon spoon;
        [SerializeField] AudioSource painterSound;
        [SerializeField] CrabRazor razor;
        [SerializeField] DragToTargetObj meat, meat2;
        [SerializeField] Transform crabLid;
        [SerializeField] Transform crabTray;

        public static int maxLayer = 30;
        public void OnCompleteCutCrab()
        {
            emoji.ShowPositive();
            scissors.OnComplete();

            dish.MoveX(10f);
            board.DOMove(new Vector2(0, -2.5f), 1f).SetDelay(.5f);
            board.DOScale(.9f, 1f).SetDelay(.5f);

            tool.MoveX(10f, 1f, false, 1f, scissors.SetupForCutLeg);
            _hint = hints[2];
        }
        public void OnCompleteCutLeg()
        {
            emoji.ShowPositive();
            scissors.OnComplete();

            stick.IsReady = true;
        }
        public void OnCompleteCrabStick()
        {
            emoji.ShowPositive();
            stick.OnComplete();

            hammer.IsReady = true;
            crabBody.transform.MoveX(-10f, 1, false, .5f);
            legs.MoveX(10f);
            _hint = hints[3];
        }
        public void OnCompleteSmashCrab()
        {
            emoji.ShowPositive();
            hammer.OnComplete();

            pry.IsReady = true;
        }
        public void OnCompleteOpenCrab()
        {
            emoji.ShowPositive();
            pry.OnComplete();

            tweezers.IsReady = true;
            _hint = hints[4];
        }
        public void OnCompleteRemoveIntestines()
        {
            emoji.ShowPositive();
            tweezers.OnComplete();

            spoon.IsReady = true;
        }
        public void StartPainter()
        {
            painterSound.gameObject.SetActive(true);
        }
        public void EndPainter()
        {
            painterSound.gameObject.SetActive(false);
        }
        public void OnCompleteCrabSauce1()
        {
            emoji.ShowPositive();
            spoon.OnCompleteSauce();
        }
        public void OnPutSauceInBowl()
        {
            emoji.ShowPositive();
            spoon.OnComplete();

            scissors.SetupForCutCrab();
            _hint = hints[5];
        }
        public void OnCompleteCutCrabBody()
        {
            emoji.ShowPositive();
            scissors.SetupForCutLung();
        }
        public void OnCompleteCutLung()
        {
            emoji.ShowPositive();
            scissors.OnComplete();

            razor.IsReady = true;
            crabBody.OnStartTrimMeat();
        }
        public void OnCompleteTrimMeat()
        {
            emoji.ShowPositive();
            razor.OnComplete();

            meat.IsReady = true;
        }
        public void OnPutMeatInBow()
        {
            emoji.ShowPositive();
            crabBody.OnComplete();

            tweezers.SetupForGapEgg();
            crabLid.MoveX(-10f, 1, false);
            _hint = hints[6];
        }
        public void OnCompleteGapEgg()
        {
            emoji.ShowPositive();
            tweezers.OnComplete();

            razor.SetupForLid();
        }
        public void OnCompleteTrimMeat2()
        {
            emoji.ShowPositive();
            razor.OnComplete();

            meat2.IsReady = true;
        }
        public void OnPutMeatInBow2()
        {
            emoji.ShowPositive();

            Camera.main.transform.DOMoveX(20f, 1f).SetDelay(.5f);
            crabTray.MoveY(10f, 1f, false, 1.5f);
            _hint = hints[7];
        }
        int crabCount = 0;
        public void OnPutCrabPartIn()
        {
            crabCount++;
            if (crabCount > 10)
            {
                emoji.ShowPositive();
                OnEndGame();
            }
        }
        [SerializeField] Transform decor1, decor2, board2;
        private void OnEndGame()
        {
            crabTray.MoveY(-10f, .5f, true, .5f);
            board2.MoveY(-5f, .5f, false, 1f);
            decor1.MoveX(10f, .5f, false, 1.5f);
            decor2.MoveX(-10f, .5f, false, 1.75f);

            // EndGame();
        }
    }
}

