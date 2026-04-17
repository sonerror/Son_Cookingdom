using DG.Tweening;
using System;
using UnityEngine;
namespace sonnv
{

    public class SnapPoint : SonMonoBehaviour
    {
        public bool canSnap = true;
        public bool isSnap;

        public void Show()
        {
            gameObject.SetActive(true);
            Vector3 scale = Tf.localScale;
            Tf.localScale = Vector3.zero;
            Tf.DOScale(scale, 0.3f);
        }
        public virtual void OnSnap()
        {
        }
        public virtual void ForceChangeSnap(bool snap)
        {
            isSnap = snap;
        }
        public void ChangeCanSnap(bool snap)
        {
            canSnap = snap;
        }
        public void ResetSnap()
        {
            isSnap = false;
            canSnap = false;
        }
    }

}
