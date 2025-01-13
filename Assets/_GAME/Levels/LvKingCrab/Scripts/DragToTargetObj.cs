using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class DragToTargetObj : KingCrabTool
    {
        [SerializeField] Transform target;

        public UnityEvent eventComplete;
        protected override void MouseDown(BaseEventData eventData)
        {
            if (!IsReady) return;
            base.MouseDown(eventData);
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady) return;

            if(Vector2.Distance(Tf.position, target.position) < dropDistance)
            {
                gameObject.SetActive(false);
                eventComplete?.Invoke();
            }
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            //base.MouseUp(eventData)
        }
    }
}

