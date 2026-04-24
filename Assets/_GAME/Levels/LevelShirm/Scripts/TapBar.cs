using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace sonnv
{
    public class TapBar : MonoBehaviour
    {
        [SerializeField] private float winTolerance = 0.12f;
        [SerializeField] private Animator anim;
        [SerializeField] private ParticleSystem slashVFX, slashMissVFX;
        [SerializeField] private Transform targetTF, pointTF;
        [SerializeField] private Vector2 targetLimit, pointLimit;
        [SerializeField] private Vector2 pointSpeed;
        // [SerializeField] private ItemAlpha itemAlpha, boderAlpha;
        [SerializeField] private AudioClip perfectClip, missClip;
        [SerializeField] private int step;
        [SerializeField] GameObject vfxObj;
        private float speed;
        bool isCanTap = false;
        public UnityEvent OnTapEvent;
        public UnityEvent OnDoneEvent;
        public UnityEvent OnFailEvent;

        public void OnInit(int step)
        {
            vfxObj.SetActive(false);
            gameObject.SetActive(true);
            anim.SetTrigger("start");
            this.step = step;
            //  boderAlpha.enabled = false;
            //  boderAlpha.DoAlpha(1, 0.15f);
        }

        public void OnDone()
        {
            isCanTap = false;
            anim.SetTrigger("complete");
            Invoke(nameof(OnDespawn), 1f);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
            OnDoneEvent?.Invoke();
        }

        [Button]
        public void OnStart()
        {
            isCanTap = true;
            pointTF.localPosition = Vector3.up * pointLimit.y;
            targetTF.localPosition = Vector3.up * Random.Range(targetLimit.x, targetLimit.y);
            speed = Random.Range(pointSpeed.x, pointSpeed.y);
            pointTF.gameObject.SetActive(true);
            //itemAlpha.DoAlpha(1, 0.15f);
        }

        public void OnTap()
        {
            if (!isCanTap) return;
            isCanTap = false;
            pointTF.gameObject.SetActive(false);
            anim.SetTrigger("hit");

            if (IsContact())
            {
                slashVFX.transform.position = pointTF.position;
                slashVFX.Play();

                if (--step <= 0)
                {
                    Invoke(nameof(OnDone), 0.5f);
                    OnTapEvent?.Invoke();
                }
                else
                {
                    //itemAlpha.DoAlpha(0, 0.15f, 0.75f);
                    Invoke(nameof(OnStart), 1f);
                    OnTapEvent?.Invoke();
                }

            }
            else if (Mathf.Abs(pointTF.localPosition.y) < 0.875f)
            {
                slashMissVFX.transform.position = pointTF.position;
                slashMissVFX.Play();
                // itemAlpha.DoAlpha(0, 0.15f, 0.75f);
                Invoke(nameof(OnStart), 1f);
                OnFailEvent?.Invoke();
            }
            else
            {
                Invoke(nameof(OnStart), 1f);
                OnFailEvent?.Invoke();
            }

        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnTap();
            }

            pointTF.localPosition += speed * Time.deltaTime * Vector3.up;
            if (pointTF.localPosition.y > pointLimit.y)
            {
                pointTF.localPosition = Vector3.up * pointLimit.x;
            }
        }

        private bool IsContact()
        {
            return Vector2.Distance(pointTF.localPosition, targetTF.localPosition) <= winTolerance;
        }

    }
}
