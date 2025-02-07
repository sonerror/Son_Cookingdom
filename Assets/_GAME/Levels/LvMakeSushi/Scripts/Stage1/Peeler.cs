using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class Peeler : MonoBehaviour
    {
        [SerializeField] DraggableObject draggable;
        [SerializeField] FxType sfxPeel = FxType.SfxPeel;
        CuttingBoard board => LevelMakeSushi.Ins.board;

        private bool isPeeling = false;
        public void CheckStartPos()
        {

        }
        public void CheckBoard()
        {
            if (isPeeling) return;
            if (!board.IsBoardEmpty
                && !board.IsVegPeeled
                && Mathf.Abs(transform.position.y - board.transform.position.y) < 1f
                && Mathf.Abs(transform.position.x - board.transform.position.x) < 1.25f)
            {
                isPeeling = true;
                draggable.LockPosition();

                SoundManager.Ins.PlayFx(sfxPeel);

                Sequence sequence = DOTween.Sequence();
                float x = 1.5f, duration = .25f;
                transform.DORotate(new Vector3(0, 0, 90f), .25f);
                sequence.Append(transform.DOMove(board.transform.position - Vector3.right * .5f, duration / 2));
                sequence.Append(transform.DOMoveX(transform.position.x + x, duration));
                sequence.Append(transform.DOMoveX(transform.position.x - x / 2, duration));
                sequence.Append(transform.DOMoveX(transform.position.x + x, duration));
                sequence.Append(transform.DOMoveX(transform.position.x - x / 2, duration));
                sequence.Append(transform.DOMoveX(transform.position.x + x, duration));
                sequence.Append(transform.DOMoveX(transform.position.x - x / 2, duration));
                sequence.Append(transform.DOMoveX(transform.position.x + x, duration));
                sequence.OnComplete(OnAnimComplete);

                void OnAnimComplete()
                {

                    Debug.Log("OnAnimComplete");
                    draggable.UnlockPosition();
                    board.OnCompletePeeling();
                    isPeeling = false;
                    transform.DORotate(new Vector3(0, 0, 0), .25f);
                    transform.DOMove(board.transform.position + Vector3.right * 1f, .1f);
                }
            }
        }
    }
}

