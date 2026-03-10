using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace sonnv
{
    public class CuttingObject : SonMonoBehaviour
    {
        [SerializeField] protected Collider2D col;
        [SerializeField] private SonTapItem tapItem;
        public SonTapItem TapItem => tapItem;
        [SerializeField] private bool isTapItem;
        [SerializeField] private bool cutDone = false;
        public bool CutDone => cutDone;
        public bool IsTapItem => isTapItem;
        public UnityEvent eventDoneActionDance;
        public void ActionCutDone()
        {
            Debug.Log("cut done Item");
            cutDone = true;
            eventDoneActionDance?.Invoke();
        }

        public void SetUp()
        {
            if (isTapItem)
            {
                tapItem.ColD.enabled = true;
                tapItem.eventOnPointDown.RemoveAllListeners();
                tapItem.eventOnPointDown.AddListener(() =>
                {
                    CuttingBoard.Instance.ResetCanSnap();
                });
            }
            else
            {
                CuttingBoard.Instance.ResetCanSnap();
            }
        }
    }
}
