using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace sonnv
{
    public class AnimSnapCream : SonMonoBehaviour
    {
        [SerializeField] private SortingOrderController changelayer;

        [SerializeField] private Transform tfRotate;
        [SerializeField] private float rotateTrans = 45f;
        [SerializeField] private AudioData onTransAudio;
        [SerializeField] private SpriteRenderer sprIng;
        [SerializeField] private UnityEvent onTrans;
        public UnityEvent OnTrans => onTrans;
        [SerializeField] FxType soundPlay = FxType.None;

        private Vector3 _initLocalPos;
        private float _initZRot;
        private Tween _rotateTween;
        private Tween _moveBackTween;

        private void Awake()
        {
            _initLocalPos = Tf.localPosition;
        }
        private void Start()
        {
            _initZRot = Tf.eulerAngles.z;
        }

        void FadeSprite(SpriteRenderer sprite, float alpha, float time, Ease ease = Ease.Linear, System.Action onDone = null)
        {
            sprite.DOFade(alpha, time)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    onDone?.Invoke();
                });

        }
        public void OnTransiton()
        {
            changelayer.IncreaseSortingOrder(70);
            Tf.DOMoveY(_initLocalPos.y + 0.5f, 0.3f).OnComplete(() =>
            {
                Tf.DOMove(tfRotate.position, 0.15f)
                    .OnComplete(() =>
                    {
                        if (soundPlay != FxType.None)
                        {
                            SoundManager.Ins.PlayFx(soundPlay);
                        }
                        Tf.DORotate(new Vector3(0, 0, rotateTrans), 0.3f).OnComplete(() =>
                            {
                                SoundManager.PlaySFX(onTransAudio.clip, onTransAudio.volume);
                                FadeSprite(sprIng, 1, 0.3f, Ease.Linear, () =>
                                {
                                    onTrans?.Invoke();
                                    FadeSprite(sprIng, 0, 0.3f, Ease.Linear, () =>
                                    {
                                        Tf.DORotate(new Vector3(0, 0, _initZRot), 0.3f).OnComplete(() =>
                                            {
                                                MoveBack();
                                            });
                                    });

                                });
                            });
                    });
            });
        }
        private void MoveBack()
        {
            _moveBackTween = Tf.DOLocalMove(_initLocalPos, 0.3f)
               .OnComplete(() =>
               {
                   _rotateTween = Tf.DORotate(
                        new Vector3(0, 0, _initZRot),
                        0.3f
                    );
                   changelayer.ResetSortingOrder();
               });

        }
    }
}
