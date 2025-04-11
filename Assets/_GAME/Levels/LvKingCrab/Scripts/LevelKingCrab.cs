
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

            state21.SetActive(true);
            state22.SetActive(false);
        }

        public EmojiControl emoji;
        [SerializeField] public Scissors scissors;
        [SerializeField] Transform dish, board, tool;
        [SerializeField] CrabStickTool stick;
        [SerializeField] public CrabBody crabBody;
        [SerializeField] public CrabHammer hammer;
        [SerializeField] Transform legs;
        [SerializeField] public CrabPry pry;
        [SerializeField] public CrabTweezers tweezers;
        [SerializeField] public CrabSpoon spoon;
        [SerializeField] public CrabRazor razor;
        [SerializeField] public DragToTargetObj meat, meat2;
        [SerializeField] Transform crabLid;
        [SerializeField] Transform crabTray;
        [SerializeField] public LegsGroup legsGroup;
        [SerializeField] public ItemDropGroup inDropGroup;
        public CrabBowl bowl;
        [SerializeField] GameObject state21;
        [SerializeField] GameObject state22;

        public ItemCrabGroup crabGroup;
        public ItemDropGroup bodyPartGroup;

        public int state = 0;


        public static int maxLayer = 30;
        public void OnCompleteCutCrab()
        {
            StartCoroutine(IE_OnCompleteCutCrab());
        }

        IEnumerator IE_OnCompleteCutCrab()
        {
            TutorialManager.Ins.MouseUpItem();
            state = 1;
            emoji.ShowPositive();
            scissors.OnComplete();
            legs.MoveX(10f, 0.3f);
            dish.MoveX(10f, 0.3f);
            yield return Cache.GetWFS(0.5f);
            board.DOMove(new Vector2(0, -2.5f), 1f).SetDelay(.5f);
            board.DOScale(.9f, 1f).SetDelay(0.5f);
            yield return Cache.GetWFS(0.5f);
            tool.MoveX(10f, 0.3f, false, 1f);
            hammer.IsReady = true;
            yield return Cache.GetWFS(0.5f);
            crabBody.transform.MoveX(-10f, 1, false, .5f);
        }
        public void OnCompleteCutLeg() // not use
        {
            emoji.ShowPositive();
            scissors.OnComplete();

            stick.IsReady = true;
        }
        public void OnCompleteCrabStick() // not use
        {
            emoji.ShowPositive();
            stick.OnComplete();

            hammer.IsReady = true;
            crabBody.transform.MoveX(-10f, 1, false, .5f);
            legs.MoveX(10f);
        }
        public void OnCompleteSmashCrab()
        {
            state = 2; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            hammer.OnComplete();

            pry.IsReady = true;
        }
        public void OnCompleteOpenCrab()
        {
            state = 3; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            pry.OnComplete();

            tweezers.IsReady = true;
        }
        public void OnCompleteRemoveIntestines()
        {

            state = 4; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            tweezers.OnComplete();

            spoon.IsReady = true;
        }
        public void StartPainter()
        {
            SoundManager.Ins.PlaySoundLoop(FxType.PaintBush);
        }
        public void EndPainter()
        {
            SoundManager.Ins.StopSoundLoop(FxType.PaintBush);
        }
        public void OnCompleteCrabSauce1()
        {
            state = 5; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            spoon.OnCompleteSauce();
        }
        public void OnPutSauceInBowl()
        {
            state = 6; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            spoon.OnComplete();

            scissors.SetupForCutCrab();
        }
        public void OnCompleteCutCrabBody()
        {
            state = 50; TutorialManager.Ins.MouseUpItem(); // 6.5

            emoji.ShowPositive();
            scissors.SetupForCutLung();
        }
        public void OnCompleteCutLung()
        {
            state = 7; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            scissors.OnComplete();

            razor.IsReady = true;
            crabBody.OnStartTrimMeat();
        }
        public void OnCompleteTrimMeat()
        {
            state = 8; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            razor.OnComplete();

            meat.IsReady = true;
        }
        public void OnPutMeatInBow()
        {

            state = 9; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            crabBody.OnComplete();


            tweezers.SetupForGapEgg();

            crabLid.MoveX(-10f, 1, false);
        }
        public void OnCompleteGapEgg()
        {

            state = 10; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            tweezers.OnComplete();

            razor.maskGroupCustom.transform.localPosition = new Vector3(0f, 0.75f, 0);
            Debug.Log("OnCompleteGapEgg");
            razor.SetupForLid();
        }
        public void OnCompleteTrimMeat2()
        {

            state = 11; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            razor.OnComplete();

            meat2.IsReady = true;
        }
        public void OnPutMeatInBow2()
        {

            state = 12; TutorialManager.Ins.MouseUpItem();
            emoji.ShowPositive();
            state22.SetActive(true);
            Camera.main.transform.DOMoveX(20f, 1f).SetDelay(.5f).OnComplete(() =>
            {
                state21.SetActive(false);

            });
            crabTray.MoveY(10f, 1f, false, 1.5f);

        }
        int crabCount = 0;
        public void OnPutCrabPartIn()
        {

            crabCount++;
            if (crabCount > 10)
            {
                emoji.ShowPositive();
                OnEndGame();

                state = 13;
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
            Debug.Log("End Game");
            GameManager.Ins.showEndGame();
        }
    }
}

