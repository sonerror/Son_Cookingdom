using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.CookV2
{
    public class APDv2MultiTargetDrag : APDv2Drag
    {
        [FoldoutGroup("Bool")][SerializeField] private bool isRemoveTargetAfterComplete = true;
        private List<Transform> _targetTf = new List<Transform>();
        private int _targetIndex;
        protected override void Start()
        {
            base.Start();
            foreach (var tar in targets)
            {
                _targetTf.Add(tar.transform);
            }
            onMouseDown.AddListener(OnReady);
        }

        protected override void CheckTarget()
        {
            int index = APDUtilities.GetNearestTranformIndex(center, _targetTf);
            if(index < 0) return;
            SwitchTarget(index);
            if (IsInRange(center.position, target.position) && IsSatisfyCondition)
            {
                if (!IsReady)
                {
                    OnIncorrectUse();
                    return;
                }
                if (isRemoveTargetAfterComplete)
                {
                    _targetTf.RemoveAt(index);
                    var list = targets.ToList();
                    list.RemoveAt(index);
                    targets = list.ToArray();
                }
                OnComplete();
            }
            else if(isMouseUpCheck) OnIncorrectUse();
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.cyan;
            foreach (var tar in targets)
            {
                Gizmos.DrawWireSphere(tar.transform.position, dropDistance);
            }
        }
#endif
    }
}
