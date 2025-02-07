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

        public bool CanCheckBroad = true;

        public DraggableObject draggable;
        public UnityEvent eventCompletePeeling, eventCompleteCutting;

        CuttingBoard cuttingBoard => LevelMakeSushi.Ins.board;

        private Vector3 prePos;

        private bool isCut = false;

        public bool IsPeeled;
        public bool IsCut => isCut;
        public bool IsComplete => isCut && IsPeeled;

        public Collider2D collide;

        protected void Awake()
        {
            prePos = transform.position;
        }

        public void CheckStartPos()
        {

        }

        public void CheckBoard()
        {
            if (!CanCheckBroad) return;
            if (!isCut && cuttingBoard.IsBoardEmpty && Vector2.Distance(transform.position, cuttingBoard.transform.position) < 1f)
            {
                draggable.LockPosition();
                cuttingBoard.OnPutVegetableIn(this);
                transform.DOMove(cuttingBoard.transform.position, .5f);
            }

        }
        public void OnComplePeeling()
        {
            IsPeeled = true;
            eventCompletePeeling?.Invoke();

            if (!tfPeel) return;
            tfPeel.SetParent(null);
            float size = tfPeel.localScale.x;
            tfPeel.DOScale(1.1f * size, .5f).OnComplete(() =>
            {
                tfPeel.DOScale(1f * size, 0.1f);
                try
                {
                    tfPeel.GetComponent<Peel>().ResetPosition();
                }
                catch (System.Exception) { }
            });
            tfPeel.DOLocalMoveY(-1.728f, .5f);
        }

        public bool isAvocado = false;
        [SerializeField] private Transform tfSeed;
        [SerializeField] private GameObject cut, peeled;
        public void OnCompleteCutting()
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                LevelMakeSushi.Ins.OnVegetablePrefareComplete();
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
                tfSeed.DOLocalMoveY(-1.2f, .5f).OnComplete(() =>
                {
                    try
                    {
                        tfSeed.GetComponent<Peel>().ResetPosition();
                    }
                    catch (System.Exception) { }
                });
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

