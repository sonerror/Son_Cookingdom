using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class SpoonIngredient : MonoBehaviour
    {
        [Serializable]
        public class SpriteToSpoon
        {
            public Sprite spriteWhenSpoonHold;
            public Sprite spriteWhenSpoonRelease;
        }

        [SerializeField] private SpriteToSpoon sprite;
        [SerializeField] private Collider2D col;
        [SerializeField] private SnapPoint snap;
        [SerializeField] private float distanceAcceptSnap = 1f;
        [SerializeField] private bool disableSpriteWhenTake;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform tftarget;
        [SerializeField] private GameObject objSmoke;
        [SerializeField] private bool isHideObjAfterSnap = false;
        [SerializeField] private bool isOnintOnEnable = false;
        [SerializeField] private bool isSnap = false;

        private bool _ignoreAfterSnap;

        public UnityEvent onShareIngredientFeedback;
        public UnityEvent onSnap;
        public UnityEvent onNotSnapWhenValidDistance;
        public SpriteToSpoon Sprite => sprite;
        public bool IgnoreThis => _ignoreAfterSnap;
        public SpriteRenderer SpriteObj => spriteRenderer;

        public Collider2D Col => col;

        [SerializeField] private bool isTypeSkewers = false;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private bool _initialSpriteEnabled;
        private bool _initialObjSmokeActive;
        public event Action OnSomethingHappened;

        private void Start()
        {
            Oninit();
        }
        private void OnEnable()
        {
            if (isOnintOnEnable)
            {
                Oninit();
            }
        }
        private void OnDestroy()
        {
        }

        private void Oninit()
        {

            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _initialSpriteEnabled = spriteRenderer.enabled;
            if (objSmoke != null)
            {
                _initialObjSmokeActive = objSmoke.activeSelf;
            }
            if (!isSnap)
            {
            }
        }
        public bool GetIsSnap()
        {
            return isSnap;
        }
        public void ResetToInitialState()
        {
            _ignoreAfterSnap = false;

            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            spriteRenderer.enabled = _initialSpriteEnabled;
            if (objSmoke != null)
            {
                objSmoke.SetActive(_initialObjSmokeActive);
            }
            this.gameObject.SetActive(false);
        }

        private void CheckBlockPlayerInteract()
        {
            if (!isSnap)
            {
                spriteRenderer.enabled = true;
                col.enabled = true;
            }
        }

        public bool TrySnap(Spoon spoon)
        {
            if (!CanSnap(spoon))
            {
                if (disableSpriteWhenTake)
                {
                    spriteRenderer.enabled = true;

                }
                return false;
            }
            snap.isSnap = true;
            _ignoreAfterSnap = true;
            if (disableSpriteWhenTake)
            {
                if (!isHideObjAfterSnap)
                {
                    gameObject.SetActive(false);
                }
                else
                {

                    col.enabled = false;
                    this.gameObject.transform.localScale = Vector3.zero;
                }
            }
            isSnap = true;
            onSnap?.Invoke();
            return true;
        }
        private void ResetAll()
        {
        }
        public void ResetAllInSpoon()
        {
            ResetAll();
            HideObjSmoke(false);

        }

        private void HideObjSmoke(bool value)
        {
            objSmoke.SetActive(value);
        }

        private bool CanSnap(Spoon spoon)
        {
            if (Vector2.Distance(snap.Tf.position, spoon.Tf.position) <= distanceAcceptSnap)
            {
                if (snap.canSnap && !snap.isSnap) return true;
                onNotSnapWhenValidDistance.Invoke();
                return false;
            }
            return false;
        }



        public void ShareIngredientFeedback()
        {
            onShareIngredientFeedback?.Invoke();
            if (disableSpriteWhenTake)
            {
                spriteRenderer.enabled = false;

            }
        }

        public Vector3 SnapPosition => snap.Tf.position;

        public Vector3 TfPositionTarget => tftarget.position;
    }

}

