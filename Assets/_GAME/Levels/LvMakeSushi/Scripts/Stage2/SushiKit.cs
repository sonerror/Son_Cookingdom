using AnhPD.MakeSushi;
using DG.Tweening;
using Satisgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class SushiKit : MonoBehaviour
    {
        [SerializeField] GameObject brushRice, brushMustard, seaweed;
        [SerializeField] SpriteRenderer[] stuff;
        [SerializeField] Sprite[] stuffSample;
        [SerializeField] SushiRoller roller;

        public SushiStuffType mainType;
        private bool
            isHaveRice = false,
            isHaveMustard = false,
            isHaveSeaweed = false,
            isHaveCarrot = false,
            isHaveCucumber = false,
            isHaveMainStuff = false;

        private int index = 0;

        private EmojiControl emoji => LevelMakeSushi.Ins.emojiStage2;

        public enum SushiStuffType
        {
            None = 0,
            Cucumber = 1,
            Carrot = 2,
            Avocado = 3,
            Fish = 4,
        }

        public void OnSeaweedReady()
        {
            if (isHaveSeaweed) return;

            brushRice.SetActive(true);
            seaweed.SetActive(true);
            isHaveSeaweed = true;
            emoji.ShowPositive();
        }
        public void OnRiceReady()
        {
            brushMustard.SetActive(true);
            isHaveRice = true;
            emoji.ShowPositive();
        }
        public void OnMustardReady()
        {
            isHaveMustard = true;
            emoji.ShowPositive();
        }
        public void PutStuffOn(SushiStuff stuff)
        {
            if (!isHaveSeaweed || !isHaveMustard || !isHaveRice)
            {
                emoji.ShowNegative();
                LevelMakeSushi.Ins.OnNotUseStuff(stuff.type);
                return;
            }
            SushiStuffType type = stuff.type;
            switch (type)
            {
                case SushiStuffType.Cucumber:
                    if (!isHaveCucumber)
                    {
                        isHaveCucumber = true;
                        OnStuffOn(stuff);
                        LevelMakeSushi.Ins.OnUseStuff(type);
                        return;
                    }
                    break;
                case SushiStuffType.Carrot:
                    if (!isHaveCarrot)
                    {
                        isHaveCarrot = true;
                        OnStuffOn(stuff);
                        LevelMakeSushi.Ins.OnUseStuff(type);
                        return;
                    }
                    break;
                case SushiStuffType.Avocado:
                    if (!isHaveMainStuff)
                    {
                        isHaveMainStuff = true;
                        mainType = stuff.type;
                        OnStuffOn(stuff);
                        LevelMakeSushi.Ins.OnUseStuff(type);
                        return;
                    }
                    break;
                case SushiStuffType.Fish:
                    if (!isHaveMainStuff)
                    {
                        isHaveMainStuff = true;
                        mainType = stuff.type;
                        OnStuffOn(stuff);
                        LevelMakeSushi.Ins.OnUseStuff(type);
                        return;
                    }
                    break;
            }
            LevelMakeSushi.Ins.OnNotUseStuff(type);
            emoji.ShowNegative();
        }
        private void OnStuffOn(SushiStuff sushiStuff)
        {
            if (index > 2) return;
            sushiStuff.isEnable = false;
            sushiStuff.draggable.LockPosition();
            sushiStuff.transform.DOMove(stuff[index].transform.position, .1f).OnComplete(() =>
            {
                stuff[index].enabled = true;
                stuff[index].sprite = stuffSample[(int)sushiStuff.type];
                index++;
                sushiStuff.draggable.UnlockPosition();

                if (index > 2)
                {
                    roller.StartRolling();
                    brushMustard.SetActive(false);
                    brushRice.SetActive(false);
                }
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(.1f);
                    sushiStuff.isEnable = true;
                }
            });
        }
    }
}

