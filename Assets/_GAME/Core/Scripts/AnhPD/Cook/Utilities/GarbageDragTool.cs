using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
    [RequireComponent(typeof(GarbageDrag))]
    public class GarbageDragTool : APDCookingDrag
    {
        [SerializeField] private GarbageDrag garbageDrag;
        public bool IsRewind = true;
        protected override void Start()
        {
            base.Start();
            IsReady = true;
            mouseDownEvent.AddListener(garbageDrag.OnPickUp);
            mouseUpEvent.AddListener(garbageDrag.OnPutDown);
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            if (IsRewind)
            {
                base.MouseUp(eventData);
            }
            else
            {
                if (!isMouseUpCheck || !IsReady) return;
                mouseUpEvent?.Invoke();
                CheckTarget();
            }
        }
        public override void InitProperties()
        {
            base.InitProperties();
            garbageDrag = GetComponent<GarbageDrag>();
        }
    }
}

