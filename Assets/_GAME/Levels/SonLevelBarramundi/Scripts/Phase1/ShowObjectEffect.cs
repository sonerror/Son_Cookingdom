using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class ShowObjectEffect : SonMonoBehaviour
    {
        [Header("Direction")]
        [SerializeField] private Direction direction = Direction.Up;
        [SerializeField] private Direction hideDirection = Direction.Down;

        [Header("Offset")]
        [SerializeField] private float xPositionShow = 2f;
        [SerializeField] private float yPositionShow = 2f;

        [Header("Fade")]
        [SerializeField] private bool fadeSprite = true;
        [SerializeField] private SpriteRenderer[] sprites;
        [SerializeField] private float timeFade = 0.2f;
        [SerializeField] private float delayFadeOut = 0.3f;

        [Header("Sorting Boost")]
        [SerializeField] private bool boostSortingLayer;
        [SerializeField] private int sortingOffset = 10;
        [SerializeField] private float detalTimer = 0.9f;

        [Header("Animation")]
        [SerializeField] private float timeShow = 0.5f;
        [SerializeField] private bool showOnEnable;
        [SerializeField] private Ease easeShow = Ease.OutBack;
        [SerializeField] private Ease easeHide = Ease.InBack;

        public UnityEvent onShow;
        public UnityEvent onHide;
        public UnityEvent onShowComplete;
        public UnityEvent onHideStart;
        public UnityEvent onHitWater;

        private Vector3 startPosition;
        private bool hasStartPosition;

        private Sequence showSeq;

        private float[] baseAlpha;
        private int[] baseSorting;

        private bool initialized;

        private void Init()
        {
            if (initialized) return;

            int len = sprites.Length;

            baseAlpha = new float[len];
            baseSorting = new int[len];

            for (int i = 0; i < len; i++)
            {
                SpriteRenderer s = sprites[i];
                baseAlpha[i] = s.color.a;
                baseSorting[i] = s.sortingOrder;
            }

            initialized = true;
        }

        private Vector3 GetStartPosition()
        {
            if (!hasStartPosition)
            {
                startPosition = Tf.localPosition;
                hasStartPosition = true;
            }

            return startPosition;
        }

        private Vector3 GetEndPosition(Direction direct)
        {
            Vector3 offset = Vector3.zero;

            switch (direct)
            {
                case Direction.Up: offset = new Vector3(0, yPositionShow, 0); break;
                case Direction.Down: offset = new Vector3(0, -yPositionShow, 0); break;
                case Direction.Left: offset = new Vector3(-xPositionShow, 0, 0); break;
                case Direction.Right: offset = new Vector3(xPositionShow, 0, 0); break;
                case Direction.UpLeft: offset = new Vector3(-xPositionShow, yPositionShow, 0); break;
                case Direction.UpRight: offset = new Vector3(xPositionShow, yPositionShow, 0); break;
                case Direction.DownLeft: offset = new Vector3(-xPositionShow, -yPositionShow, 0); break;
                case Direction.DownRight: offset = new Vector3(xPositionShow, -yPositionShow, 0); break;
            }

            return GetStartPosition() + offset;
        }

        private void SetAlpha(float value)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                Color c = sprites[i].color;
                c.a = value;
                sprites[i].color = c;
            }
        }

        private void SetSortingBoost(bool boost)
        {
            if (!boostSortingLayer) return;

            int len = sprites.Length;

            if (boost)
            {
                for (int i = 0; i < len; i++)
                    sprites[i].sortingOrder = baseSorting[i] + sortingOffset;
            }
            else
            {
                for (int i = 0; i < len; i++)
                    sprites[i].sortingOrder = baseSorting[i];
            }
        }

        [Button]
        public void Show()
        {
            Init();

            gameObject.SetActive(true);

            showSeq?.Kill();

            onShow?.Invoke();

            Tf.localPosition = GetEndPosition(direction);

            if (fadeSprite)
                SetAlpha(0f);

            SetSortingBoost(true);

            showSeq = DOTween.Sequence();

            showSeq.Append(
                Tf.DOLocalMove(GetStartPosition(), timeShow)
                .SetEase(easeShow)
            );

            if (fadeSprite)
            {
                showSeq.Join(
                    DOVirtual.Float(0, 1, timeFade, SetAlpha)
                );
            }

            float restoreTime = timeShow * detalTimer;

            showSeq.InsertCallback(restoreTime, () =>
            {
                onHitWater?.Invoke();
                SetSortingBoost(false);
            });

            showSeq.OnComplete(() =>
            {
                onShowComplete?.Invoke();
            });
        }

        [Button]
        public void Hide()
        {
            Init();

            onHideStart?.Invoke();

            showSeq?.Kill();

            showSeq = DOTween.Sequence();

            showSeq.Append(
                Tf.DOLocalMove(GetEndPosition(hideDirection), timeShow)
                .SetEase(easeHide)
            );

            if (fadeSprite)
            {
                showSeq.Join(
                    DOVirtual.Float(1, 0, timeFade, SetAlpha)
                    .SetDelay(delayFadeOut)
                );
            }

            showSeq.OnComplete(() =>
            {
                gameObject.SetActive(false);
                onHide?.Invoke();
            });
        }

        private void OnEnable()
        {
            if (showOnEnable)
                Show();
        }
        public void Show(float delay)
        {
            DOVirtual.DelayedCall(delay, Show);
        }
        public void Hide(float delay)
        {
            DOVirtual.DelayedCall(delay, Hide);
        }
    }
}