using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD
{
    public class SimpleTransition : MonoBehaviour
    {
        [SerializeField] Transform mask;
        public void StartTransition(float transDuration = 1f, float stayDuration = 1f)
        {
            gameObject.SetActive(true);
            mask.localScale = Vector3.zero;
            mask.DOScale(1f, transDuration);

            // this.WaitToDo(close, stayDuration + transDuration);

            void close()
            {
                mask.DOScale(0f, transDuration).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            }
        }

    }
}

