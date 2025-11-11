using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
    public class DirtyTowel : APDCookingToolBase
    {
        [SerializeField] private DirtyManager dirtyManager;
        [SerializeField] private SpriteRenderer fixedSprite;

        private DirtyClean[] dirties => dirtyManager.Dirties;
        protected override void Start()
        {
            base.Start();
            IsReady = true;
        }
        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);
            fixedSprite.enabled = false;
            spriteRenderer.enabled = true;
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady) return;
            for (int i = 0; i < dirties.Length; i++)
            {
                if (IsInRange(dirties[i].transform))
                {
                    dirties[i].OnCleaning();
                }
            }
        }
        protected override void Rewind(Action completeAction = null)
        {
            base.Rewind(complete);
            void complete()
            {
                completeAction?.Invoke();
                fixedSprite.enabled = true;
                spriteRenderer.enabled = false;
            }
        }
    }
}

