using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.MakeSushi
{
    public class Vegetable : MonoBehaviour
    {
        [SerializeField] Transform tfPeel;

        public Level127DraggableObject draggable;
        public UnityEvent eventCompletePeeling, eventCompleteCutting;

        CuttingBoard cuttingBoard => LevelMakeSushi.Instance.board;

        private Vector3 prePos;

        private bool isCorrectPosition = false;
        private bool isCut = false;

        public bool IsPeeled;
        public bool IsCut => isCut;
        public bool IsComplete => isCorrectPosition && isCut && IsPeeled;

        public Collider2D collide;

        protected void Awake()
        {
            prePos = transform.position;
        }

        public void CheckStartPos()
        {
            if (Vector2.Distance(transform.localPosition, Vector2.zero) < 1f)
            {
                // transform.DOLocalMove(Vector2.zero, .1f).OnComplete(() =>
                // {
                if (isCut)
                {
                    draggable.LockPosition();
                }
                // });
                isCorrectPosition = true;

            }
            else
            {
                isCorrectPosition = false;
            }
        }

        public void CheckBoard()
        {
            if (!isCut && cuttingBoard.IsBoardEmpty && Vector2.Distance(transform.position, cuttingBoard.transform.position) < 1f)
            {
                draggable.LockPosition();
                cuttingBoard.OnPutVegetableIn(this);
                transform.DOMove(cuttingBoard.transform.position, .5f).OnComplete(() =>
                {
                    CheckStartPos();
                });
            }
            else
            {
                // transform.DOMove(prePos, .5f);
            }
        }
        public void OnComplePeeling()
        {
            IsPeeled = true;
            eventCompletePeeling?.Invoke();
            tfPeel.SetParent(null);

            float size = tfPeel.localScale.x;

            tfPeel.DOScale(1.1f * size, .4f).OnComplete(() =>
            {
                tfPeel.DOScale(1f * size, .1f);
            });
            tfPeel.DOLocalMoveY(-1.628f, .5f);
        }

        public bool isAvocado = false;
        [SerializeField] private Transform tfSeed;
        [SerializeField] private GameObject cut, peeled;
        public void OnCompleteCutting()
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                LevelMakeSushi.Instance.OnVegetablePrefareComplete();
            });
            if (isAvocado)
            {
                transform.DOScale(1.1f, .1f).OnComplete(() =>
                {
                    transform.DOScale(1f, .1f);
                });
                cut.SetActive(true);
                peeled.SetActive(false);
                isAvocado = false;
                tfSeed.SetParent(null);
                return;
            }
            isCut = true;
            eventCompleteCutting?.Invoke();
            transform.DOScale(1.1f, .1f).OnComplete(() =>
            {
                transform.DOScale(1f, .1f);
            });
            draggable.UnlockPosition();
        }
    }
}

