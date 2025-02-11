using DG.Tweening;
using Satisgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class CuttingBoard : MonoBehaviour
    {
        [SerializeField] private Animator cutting;
        public bool IsBoardEmpty => veg == null;
        public bool IsVegPeeled => veg.IsPeeled;
        public Vegetable veg;

        public GameObject gb1, gb2, gb3, gb4;

        private EmojiControl emoji => LevelMakeSushi.Ins.emojiStage1;

        public void OnPutVegetableIn(Vegetable vegetable)
        {
            if (veg == null)
            {
                veg = vegetable;
                veg.draggable.LockPosition();
                veg.transform.DOMove(transform.position, .1f);
                emoji.ShowPositive();
                veg.collide.enabled = false;
            }
        }
        public void OnCompletePeeling()
        {
            veg.OnComplePeeling();
        }
        public void OnStartCutting(Knife knife)
        {
            knife.gameObject.SetActive(false);
            knife.transform.DOMoveY(knife.transform.position.y + 1f, 0.2f);
            // cutting.SetTrigger("Cut");
            StartCoroutine(playAnim());
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(.5f);
                knife.OnCompleteCutting();
                veg.OnCompleteCutting();
                if (veg.IsCut)
                {
                    veg = null;
                }
            }
        }

        private IEnumerator playAnim()
        {
            gb1.SetActive(true);
            yield return Cache.GetWFS(0.1f);
            gb2.SetActive(true);
            yield return Cache.GetWFS(0.1f);
            gb1.SetActive(false);
            gb2.SetActive(false);
            yield return Cache.GetWFS(0.1f);
            gb3.SetActive(true);
            yield return Cache.GetWFS(0.1f);
            gb4.SetActive(true);
            gb3.SetActive(false);
            gb1.SetActive(true);
            yield return Cache.GetWFS(0.1f);
            gb4.SetActive(false);
            gb1.SetActive(false);
        }
    }
}

