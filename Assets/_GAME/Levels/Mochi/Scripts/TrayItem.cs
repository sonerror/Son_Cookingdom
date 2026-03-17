using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class TrayItem : MonoBehaviour
    {
        [SerializeField] private bool isTrigger = false;
        public bool IsTrigger => isTrigger;

        [SerializeField] private ItemType itemType;
        public ItemType ItemType => itemType;

        public ItemInTray itemOrder;
        [SerializeField] private Collider2D col;
        public Collider2D Col => col;

        [SerializeField] private UnityEvent onShow;
        public UnityEvent OnShow => onShow;
        [SerializeField] private AudioClip sfxShow;
        public void OnSpoonTouch(SpoonTrigger spoon)
        {
            if (!isTrigger)
            {
                SoundManager.PlaySFXOneShot(sfxShow);
                Debug.Log("Spoon touch: " + itemType);
                itemOrder.gameObject.SetActive(true);
                isTrigger = true;
                onShow?.Invoke();
            }
        }

        public void ResetTrigger(bool value)
        {
            isTrigger = value;
            Col.enabled = !value;
        }
    }
}