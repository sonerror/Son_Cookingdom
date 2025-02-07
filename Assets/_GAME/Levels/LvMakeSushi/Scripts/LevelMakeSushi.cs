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
        [SerializeField] private RiceCooker cooker;
        [SerializeField] private CookerButton cookerButton;
        [SerializeField] private WaterValue value;
        [SerializeField] private Vegetable[] vegs;
        [SerializeField] private Knife knife;
        [SerializeField] private Peeler peeler;
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
            for (int i = 0; i < vegs.Length; i++)
            {
                if (!vegs[i].IsComplete)
                {
                    return;
                }
            }

            if (garbageCount < 4 || !cookerButton.IsCooked)
            {
                return;
            }

            cookerButton.OnComplete();
        }
        public async void OnCompleteStage1()
        {
            emojiStage1.ShowPositive();
            await Task.Delay(500);
            Camera.main.transform.DOMoveX(15f, 1f);
        }

        [Header("Stage2")]
        [SerializeField] private SushiKit sushiPrefab;
        [SerializeField] private SushiKit currentSushi;
        [SerializeField] private SushiRoll rollPrefab;
        [SerializeField] Transform tfKit;
        [SerializeField] GameObject seaweed;

        public SushiPos sushiPos;
        public EmojiControl emojiStage2, emojiStage2_dish;

        private int rollCount = 0;
        public void OnPutStuff(SushiStuff stuff)
        {
            currentSushi.PutStuffOn(stuff);
        }
        public void OnSeaweedReady()
        {
            currentSushi.OnSeaweedReady();

            // this.WaitToDo(() => { seaweed.SetActive(false); }, .1f);

            DOVirtual.DelayedCall(1f, () =>
            {
                seaweed.SetActive(false);
            });
        }
        public void OnSushiComplete(Vector2 pos)
        {
            emojiStage2.ShowPositive();

            SpawnRoll(pos);
            OnReroll();
        }
        private void OnReroll()
        {
            seaweed.SetActive(true);
            currentSushi.gameObject.SetActive(false);
            currentSushi = Instantiate(sushiPrefab, tfKit);
            currentSushi.gameObject.SetActive(true);
        }

        private SushiRoll currentRoll;
        [SerializeField] Transform tfCatHand;
        private void SpawnRoll(Vector2 pos)
        {
            rollCount++;

            currentRoll = ObjectPoolDictArray.Ins.GetGameObject(rollPrefab);
            currentRoll.transform.position = new Vector3(pos.x, pos.y, -1f);
            currentRoll.OnInit(currentSushi.mainType);

            if (rollCount == 3)
            {
                currentRoll.draggable.LockPosition();
                tfCatHand.gameObject.SetActive(true);
                tfCatHand.DOMove(currentRoll.transform.position, .5f).OnComplete(() =>
                {
                    tfCatHand.DOLocalMoveX(-15f, .5f);
                    currentRoll.transform.DOLocalMoveX(-15f, .5f).OnComplete(() =>
                    {
                        currentRoll.gameObject.SetActive(false);
                    });
                });
            }
        }
        [SerializeField] GameObject dish2, dish3;
        public void OnCompleteStage2()
        {
            dish2.SetActive(false);
            dish3.SetActive(true);

            stages[3].SetActive(true);
            stages[3].transform.localPosition = new Vector3(10f, 0, 0);
            stages[3].transform.DOLocalMoveX(0f, 1f);

            stages[2].transform.DOLocalMoveX(-10f, 1f).OnComplete(() =>
            {
                stages[2].SetActive(false);
            });

            List<GameObject> rolls = ObjectPoolDictArray.Ins.GetActiveObjects(rollPrefab.gameObject);
            for (int i = 0; i < rolls.Count; i++)
            {
                ObjectPoolDictArray.Ins.ReleaseGameObject(rolls[i]);
            }
        }

        [SerializeField] private StuffAmount[] stuffAmounts;
        public void OnTakeOffStuff(SushiKit.SushiStuffType type)
        {
            stuffAmounts[(int)type].OnTakeOffStuff();
        }
        public void OnNotUseStuff(SushiKit.SushiStuffType type)
        {
            stuffAmounts[(int)type].OnPutInStuff(false);
        }
        public void OnUseStuff(SushiKit.SushiStuffType type)
        {
            stuffAmounts[(int)type].OnPutInStuff(true);
        }

        [Header("Stage3")]
        [SerializeField]
        private GameObject goKnife;
        [SerializeField] private GameObject goRollUnCut, goRollCut, goDisk, goBG;
        [SerializeField] private Animation animSlash;

        public EmojiControl emojiStage3, emojiStage3_dish;

        [SerializeField] Collider2D[] uncutRollCollide;
        [SerializeField] GameObject[] cutRoll;
        int rollIndex = -1;
        private bool isRollInBoard = false;
        public void OnPutRollInBoard(int index)
        {
            isRollInBoard = true;
            emojiStage3.ShowPositive();
            rollIndex = index;

            for (int i = 0; i < uncutRollCollide.Length; i++)
            {
                if (i != index)
                {
                    uncutRollCollide[i].enabled = false;
                }
            }
        }
        public void CheckBoard()
        {
            if (isRollInBoard)
            {
                isRollInBoard = false;
                goKnife.SetActive(false);
                animSlash.Play();
                emojiStage3.ShowPositive();

                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(.5f);
                    goKnife.SetActive(true);
                    goKnife.transform.position += Vector3.up * 1f;

                    uncutRollCollide[rollIndex].gameObject.SetActive(false);
                    cutRoll[rollIndex].SetActive(true);

                    cutRoll[rollIndex].transform.DOScale(1.1f, .1f);
                    cutRoll[rollIndex].transform.DOScale(1f, .1f).SetDelay(.1f);

                    for (int i = 0; i < uncutRollCollide.Length; i++)
                    {
                        if (i != rollIndex)
                        {
                            uncutRollCollide[i].enabled = true;
                        }
                    }
                }
            }
        }
        int decorCount = 0;
        public void OnPutDecor()
        {
            decorCount++;
            if (decorCount > 4)
            {
                emojiStage3_dish.ShowPositive();
                OnEndGame();
            }
        }
        private void OnEndGame()
        {
            goBG.transform.DOLocalMoveX(-15f, .5f);
            dish3.transform.DOLocalMoveX(-10f, .5f);
            goDisk.transform.DOLocalMove(new Vector3(-0.1f, .23f, -1f), .5f).SetDelay(0.5f);
            EndGame();
        }

        public void EndGame()
        {
            Debug.Log("End Game");
        }
    }

}
