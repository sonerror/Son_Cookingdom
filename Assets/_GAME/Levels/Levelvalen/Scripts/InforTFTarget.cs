using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class InforTFTarget : SonMonoBehaviour
    {
        [SerializeField] private bool isSnap = false;
        [SerializeField] private ShowObjectEffect effect;
        public ShowObjectEffect Effect => effect;
        public bool IsSnap => isSnap;
        public void ChangeIsSnap(bool value)
        {
            isSnap = value;

        }
        private IEnumerator DelayAutoMove()
        {
            yield return new WaitForSeconds(0.5f);
            effect.Hide();
        }

    }
}
