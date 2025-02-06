using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class Knife : MonoBehaviour
    {
        [SerializeField] Level127DraggableObject draggable;
        [SerializeField] AudioClip sfxPeel;

        CuttingBoard board => LevelMakeSushi.Instance.board;
        private bool isCutting = false;
        private bool isCorrectPosition = false;
        public bool IsCorrectPosition => isCorrectPosition;

        Vector2 startPos;

        private void Start()
        {
            startPos = transform.position;
        }

        public void CheckBoard()
        {
            if (isCutting) return;
            if (!board.IsBoardEmpty
                && board.IsVegPeeled
                && Mathf.Abs(transform.position.y - board.transform.position.y) < .5f
                && Mathf.Abs(transform.position.x - board.transform.position.x) < 1f)
            {
                isCutting = true;
                board.OnStartCutting(this);
            }
        }
        public void CheckStartPos()
        {
            // if (Vector2.Distance(transform.position, startPos) < .5f)
            // {
            //     transform.DOMove(startPos, .1f);
            //     isCorrectPosition = true;
            //     LevelMakeSushi.Instance.CheckCompleteStage1();
            // }
            // else
            // {
            //     isCorrectPosition = false;
            // }
        }
        public void OnCompleteCutting()
        {
            gameObject.SetActive(true);
            isCutting = false;
            transform.DOMove(startPos, .5f);
            isCorrectPosition = true;

        }
    }
}

