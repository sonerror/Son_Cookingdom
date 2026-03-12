using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace sonnv
{
    public class CuttingObject : SonMonoBehaviour
    {
        [SerializeField] private KnifeCutType typeCut;
        public KnifeCutType TypeCut => typeCut;
        [SerializeField] protected Collider2D col;
        [SerializeField] private FlourMoveToCream tapToMove;
        public FlourMoveToCream TapItem => tapToMove;
        [SerializeField] private bool isTapItem;
        [SerializeField] private bool cutDone = false;
        [SerializeField] private float delayMove = 0.5f;
        public bool CutDone => cutDone;
        public bool IsTapItem => isTapItem;
        public UnityEvent eventDoneActionDance;
        public void ActionCutDone()
        {
            Debug.Log("cut done Item");
            cutDone = true;
            eventDoneActionDance?.Invoke();
            StartCoroutine(DelayAutoMove());
        }
        private IEnumerator DelayAutoMove()
        {
            yield return new WaitForSeconds(delayMove);
            AutoMoveTarget();
        }
        private void AutoMoveTarget()
        {
            tapToMove.OnMove();
        }
        public void SetUp()
        {
            CuttingBoard.Instance.ResetCanSnap();
        }
        public void SetDataTargetMove(InforTFTarget _tfTarget)
        {
            if (_tfTarget != null)
            {
                tapToMove.SetData(_tfTarget);

            }
        }
    }
}
