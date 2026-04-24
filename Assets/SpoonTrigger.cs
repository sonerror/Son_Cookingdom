using UnityEngine;

namespace sonnv
{
    public class SpoonTrigger : MonoBehaviour
    {
        [SerializeField] private TriggerToRotate targetPan;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (targetPan == null) return;
            if (other.GetComponent<TriggerToRotate>() != targetPan) return;
            targetPan.StartAutoRotate();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (targetPan == null) return;
            if (other.GetComponent<TriggerToRotate>() != targetPan) return;
            targetPan.StopAutoRotate();
        }
    }
}