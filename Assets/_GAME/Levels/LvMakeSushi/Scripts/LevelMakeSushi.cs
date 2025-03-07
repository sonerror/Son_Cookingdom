using DG.Tweening;
using Satisgame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace AnhPD.MakeSushi
{
    public class LevelMakeSushi : Singleton<LevelMakeSushi>
    {
        protected override void Awake()
        {
            base.Awake();
        }
        [SerializeField] private GameObject[] stages;
        [Header("Stage 1")]
        [SerializeField] public RiceCooker cooker;
        [SerializeField] public CookerButton cookerButton;
        [SerializeField] public WaterValue value;
        [SerializeField] public Vegetable[] vegs;
        [SerializeField] public Knife knife;
        [SerializeField] public Peeler peeler;
        public EmojiControl emojiStage1;

        public CuttingBoard board;

        private int garbageCount = 0;
        public void OnGarbageThrowed()
        {
            garbageCount++;
            CheckCompleteStage1();
        }

        private int vegetableCount = 0;
        public void OnVegetablePrefareComplete()
        {
            vegetableCount++;
            if (vegetableCount == 2)
            {
                emojiStage1.ShowPositive();

                vegs[0].transform.parent.transform.DOLocalMoveX(10f - 1.35f, 1f).OnComplete(() =>
                {
                    vegs[0].transform.parent.gameObject.SetActive(false);
                });
                vegs[1].transform.parent.transform.DOLocalMoveX(10f + 1.35f, 1f).OnComplete(() =>
                {
                    vegs[1].transform.parent.gameObject.SetActive(false);
                });

                vegs[2].transform.parent.transform.DOLocalMoveX(-10f - 1.35f, 0f);
                vegs[3].transform.parent.transform.DOLocalMoveX(-10f + 1.35f, 0f);

                vegs[2].transform.parent.gameObject.SetActive(true);
                vegs[3].transform.parent.gameObject.SetActive(true);

                vegs[2].transform.parent.transform.DOLocalMoveX(-1.35f, 1f);
                vegs[3].transform.parent.transform.DOLocalMoveX(1.35f, 1f);
            }
            if (vegetableCount > 3)
            {
                emojiStage1.ShowPositive();
                CheckCompleteStage1();
            }
        }
        public void OnWaterStart()
        {
            if (cooker.IsInSink)
            {
                cooker.OnWater();
            }
        }

        public void LockCanCheckBroadVegetable()
        {
            for (int i = 0; i < vegs.Length; i++)
            {
                vegs[i].CanCheckBroad = false;
            }
        }

        public void UnlockCanCheckBroadVegetable()
        {
            for (int i = 0; i < vegs.Length; i++)
            {
                vegs[i].CanCheckBroad = true;
            }
        }
        public void OnPutCookerInSink()
        {
            value.OnHaveObjectInSink();
            if (value.IsWatering)
            {
                cooker.OnWater();
            }
        }
        public void OnCookerLeaveSink()
        {
            value.OnRemovedAllObjectInSink();
        }
        public void CheckCompleteStage1()
        {
            Debug.Log("Complete Stage 0");

            for (int i = 0; i < vegs.Length; i++)
            {
                if (!vegs[i].IsComplete)
                {
                    return;
                }
            }
            Debug.Log("Complete Stage 0.5");
            if (garbageCount < 4 || !cookerButton.IsCooked)
            {
                return;
            }
            Debug.Log("Complete Stage 1");
            cookerButton.OnComplete();
        }
        public async void OnCompleteStage1()
        {
            emojiStage1.ShowPositive();
            stages[2].SetActive(true);
            await Task.Delay(500);
            Camera.main.transform.DOMoveX(24.1f, 1f).OnComplete(() =>
            {
                stages[1].SetActive(false);
                TutorialManager.Ins.PlayStateEndGame();
            });
        }

    }

}
