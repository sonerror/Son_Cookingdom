using DG.Tweening;
using Satisgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class CuttingBoard : MonoBehaviour
    {
        [SerializeField] private Animation cuttingAnim;
        public bool IsBoardEmpty => veg == null;
        public bool IsVegPeeled => veg.IsPeeled;
        private Vegetable veg;

        private EmojiControl emoji => LevelMakeSushi.Instance.emojiStage1;

        public void OnPutVegetableIn(Vegetable vegetable)
        {
            if( veg == null )
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
            knife.transform.DOMoveY(knife.transform.position.y + 1f,.2f);
            cuttingAnim.Play();
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
    }
}

