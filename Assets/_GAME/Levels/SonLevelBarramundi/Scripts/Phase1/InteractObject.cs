using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class InteractObject : SonMonoBehaviour
    {
        [SerializeField] private Collider2D col;
        [SerializeField] private UnityEvent onMouseUp;
        [SerializeField] private bool isDestroyAfterUse;

        public Collider2D Col => col;

        private void OnMouseUpAsButton()
        {
            //if (!LevelBase.instance.IsAllowInteract) return;
            onMouseUp.Invoke();
            if (isDestroyAfterUse)
            {
                Destroy(gameObject);
            }
        }

        public void AddListener(UnityAction action)
        {
            onMouseUp.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            onMouseUp.RemoveListener(action);
        }

        public void RemoveAllListener()
        {
            onMouseUp.RemoveAllListeners();
        }
    }

}

