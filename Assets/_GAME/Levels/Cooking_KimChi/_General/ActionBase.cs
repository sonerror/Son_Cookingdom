using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public abstract class ActionBase : MonoBehaviour
    {
        public string note;

        public UnityEvent doneEvent;
        [SerializeField] protected bool startActive = true, doneActive = true;
        [SerializeField] public float delay;
        [SerializeField] protected AudioClip clip;

        public abstract void OnActive();
        public virtual void OnStop() { }

        protected virtual void OnDone()
        {
            gameObject.SetActive(doneActive);
            doneEvent?.Invoke();
        }

        public void SetDelayTime(float time)
        {
            delay = time;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        protected virtual void Setup()
        {

        }

        protected void PlayFx()
        {
            if (clip != null)
            {
                SoundManager.PlaySFXOneShot(clip);
            }
        }
    }

}
