using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.KingCrab
{
    public class CrabBody : MonoBehaviour
    {

        public Transform Tf;

        [SerializeField] Transform bodyTop;
        [SerializeField] GameObject sauce, meat;
        [SerializeField] Transform left, right;
        [SerializeField] SpriteRenderer lungL, lungR;
        [SerializeField] GameObject meatL, meatR;
        int smashCount = 0;
        public void OnSmash()
        {
            // smashCount++;
            transform.DOPunchScale(Vector3.one * .2f, 0.3f);
            // if (smashCount > 2)
            // {
            LevelKingCrab.Instance.OnCompleteSmashCrab();
            // }
        }
        public void OnOpen()
        {
            //bodyTop.DOLocalRotate(new Vector3(55f, 0, 0), .3f);
            bodyTop.DOLocalMoveY(4f, .3f).SetDelay(.2f);
            bodyTop.MoveX(10f, 1f, true, .5f);

            sauce.SetActive(true);
        }
        public void OnCut()
        {
            left.DOMoveX(left.transform.position.x - 1f, .3f).SetEase(Ease.Linear);
            right.DOMoveX(right.transform.position.x + 1f, .3f).SetEase(Ease.Linear);
        }
        public void OnCutLungLeft()
        {
            lungL.transform.DOMoveX(lungL.transform.position.x - 3f, .5f).SetEase(Ease.Linear);
            lungL.DOFade(0, .5f);
        }
        public void OnCutLungRight()
        {
            lungR.transform.DOMoveX(lungR.transform.position.x + 3f, .5f).SetEase(Ease.Linear);
            lungR.DOFade(0, .5f);
        }
        public void OnStartTrimMeat()
        {
            meat.SetActive(true);
            meatL.SetActive(false);
            meatR.SetActive(false);
        }
        public void OnComplete()
        {
            left.MoveX(-5f, 0.5f);
            right.MoveX(5f, 0.5f, action: () =>
            {
                LevelKingCrab.Instance.razor.maskGroupCustom.ResetMask();
            });
            meat.SetActive(false);
            // Destroy(meat);

        }
    }
}

