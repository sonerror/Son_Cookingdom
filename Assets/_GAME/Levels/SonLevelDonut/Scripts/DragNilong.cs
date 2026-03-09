using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class DragNilong : MonoBehaviour
    {
        public int moveOrder, unmoveOrder;
        public SpriteRenderer render;

        private void Start()
        {
            SetUnmoveOrder();
        }

        private void OnMouseDown()
        {
            SetMoveOrder();
        }
        private void OnMouseUp()
        {
            SetUnmoveOrder();
        }

        public void CancelDragging()
        {
            OnMouseUp();
        }
        public void SetUnmoveOrder()
        {
            SetOrderLayer(unmoveOrder);
        }
        public void SetMoveOrder()
        {
            SetOrderLayer(moveOrder);
        }
        public void SetOrderLayer(int order)
        {
            render.sortingOrder = order;
        }

    }

}
