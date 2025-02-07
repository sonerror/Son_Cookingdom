using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class Knife : MonoBehaviour
    {
        [SerializeField] DraggableObject draggable;

        CuttingBoard board => LevelMakeSushi.Ins.board;
        private bool isCutting = false;

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
                && Mathf.Abs(transform.position.y - board.transform.position.y) < 1f
                && Mathf.Abs(transform.position.x - board.transform.position.x) < 1.25f)
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
            //     LevelMakeSushi.Ins.CheckCompleteStage1();
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
        }
    }
}

