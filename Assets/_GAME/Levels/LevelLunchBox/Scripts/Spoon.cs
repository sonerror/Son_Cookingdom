using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace sonnv
{
    public class Spoon : SonMonoBehaviour,
        IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private EmojiControl emoji;
        [SerializeField] private EmojiControl emoji2;
        [SerializeField] private List<SpoonIngredient> acceptIngredients;
        [SerializeField] private float distanceToGiveIngredient = 1f;
        [SerializeField] private BoxCollider2D col;
        [SerializeField] private SpriteRenderer spoonSprite;
        [SerializeField] private Sprite spoonSpriteOnNormal;
        [SerializeField] private Sprite spoonSpriteOnReleaseIngredient;
        [SerializeField] private SpriteRenderer ingredientSprite;
        [SerializeField] private int sortingOrderWhenDrag = 10;
        [SerializeField] private float zRotate = 10f;
        [SerializeField] private float zRotateAnim = 10f;
        [SerializeField] private bool jumpMoveBack;
        [SerializeField] private float yAddedFromLocalPos = 3f;
        [SerializeField] private AudioClip pickSound;
        [SerializeField] private AudioClip takeIngredientSound;
        [SerializeField] private AudioClip giveIngredientSound;
        [SerializeField] private bool checkTriggerWhenPick;
        [SerializeField] private bool isSpriteFont;
        [SerializeField] private bool isDelayResetAfterSnap = false;
        [SerializeField] private bool isMoveTargetToSnap = false;
        [SerializeField] private bool isBlockTrySnap = false;
        public void SetIsBlockTrySnap(bool value)
        {
            isBlockTrySnap = value;
        }
        [SerializeField] private TriggerToRotate triggerToRotate;
        [SerializeField] private SpriteRenderer spriteFont;

        public UnityEvent onStartDrag;
        public UnityEvent onEndMoveBack;
        public UnityEvent onStartMoveBack;
        public UnityEvent onMoveBackAfterSnap;
        public bool isManualBlock;

        protected SpoonIngredient _ingredient;
        private bool _isDragging;
        private bool _isSnap;
        private Vector3 _initLocalPos;
        private Vector2 _lastPosition;
        private Camera _cam;
        private int _sortingOrder;
        private float _initZRotate;
        private Tween _rotateTween;
        private Tween _actionTween;
        private bool _isPerformingAction = false;

        private readonly Dictionary<Collider2D, SpoonIngredient> _cachedIngredients = new();
        public Collider2D Col => col;

        protected virtual void Awake()
        {
            _cam = Camera.main;
            _initLocalPos = Tf.localPosition;
            _sortingOrder = spoonSprite.sortingOrder;
            _initZRotate = Tf.localEulerAngles.z;
        }
        private void OnDestroy()
        {

        }

        private void CheckBlockPlayerInteract()
        {
        }

        public void SetManualBlock(bool isBlock)
        {
            isManualBlock = isBlock;
            col.enabled = !isBlock;
            if (isBlock && _isDragging)
            {
                CancelDragging(true);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isPerformingAction)
            {
                _actionTween?.Kill();
                _rotateTween?.Kill();
                ReleaseIngredient();
                Tf.localPosition = _initLocalPos;
                Tf.localEulerAngles = new Vector3(0, 0, _initZRotate);
                SetSortingOrder(_sortingOrder);
            }
            _isPerformingAction = false;

            _isDragging = true;
            SetSortingOrder(sortingOrderWhenDrag);
            _lastPosition = _cam.ScreenToWorldPoint(Input.mousePosition);
            Rotate(zRotate);
            SoundManager.PlaySFX(pickSound);
            if (checkTriggerWhenPick)
            {
                CheckTriggerOnPick();
            }
            onStartDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging) return;
            if (isManualBlock) return;
            Vector2 currentPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 delta = currentPos - _lastPosition;
            Tf.position += (Vector3)delta;
            _lastPosition = currentPos;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isDragging) return;

            CancelDragging(false);
        }

        protected virtual void CancelDragging(bool forceCancel)
        {
            _isDragging = false;
            if (triggerToRotate.IsBlockRotate == false)
            {
                MoveBack();
                Rotate(_initZRotate);
                return;
            }
            else
            {
                if (isBlockTrySnap)
                {
                    // emoji.ShowNegative();
                    ReleaseIngredient();
                    MoveBack();
                    Rotate(_initZRotate);
                }
                else
                {
                    if (forceCancel)
                    {
                        _actionTween?.Kill();
                        _rotateTween?.Kill();
                        ReleaseIngredient();
                        Tf.localPosition = _initLocalPos;
                        Tf.localEulerAngles = new Vector3(0, 0, _initZRotate);
                        SetSortingOrder(_sortingOrder);
                        _isPerformingAction = false;
                        onEndMoveBack?.Invoke();
                        col.enabled = !isManualBlock;
                        return;
                    }

                    if (_ingredient)
                    {
                        ChangeIngredientStatus();
                    }
                    else
                    {
                        MoveBack();
                        Rotate(_initZRotate);
                    }
                }
            }
        }

        private void ChangeIngredientStatus()
        {
            if (_ingredient.TrySnap(this))
            {
                PlayGiveIngredientAnim();
            }
            else
            {
                emoji.ShowNegative();
                ReleaseIngredient();
                MoveBack();
                Rotate(_initZRotate);
            }
        }

        private void PlayGiveIngredientAnim()
        {
            col.enabled = false;
            _isPerformingAction = true;
            _actionTween?.Kill();
            Vector3 tf = _ingredient.SnapPosition;
            if (isMoveTargetToSnap)
            {
                tf = _ingredient.TfPositionTarget;
            }
            _actionTween = Tf.DOMove(tf, 0.3f).OnComplete(() =>
            {
                _rotateTween?.Kill();
                _rotateTween = Tf.DOLocalRotate(new Vector3(0, 0, zRotateAnim), 0.3f).OnComplete(() =>
                {
                    SoundManager.PlaySFX(giveIngredientSound);
                    spoonSprite.sprite = spoonSpriteOnReleaseIngredient;
                    ingredientSprite.sprite = _ingredient.Sprite.spriteWhenSpoonRelease;
                    if (isDelayResetAfterSnap)
                    {
                        _actionTween = ingredientSprite.DOFade(0, 0.3f);
                        StartCoroutine(IE_DelayReset());
                    }
                    else
                    {
                        _actionTween = ingredientSprite.DOFade(0, 0.3f).OnComplete(() =>
                        {
                            ReSetIngredientCook();
                        });
                    }

                });
            });
        }
        IEnumerator IE_DelayReset()
        {
            yield return new WaitForSeconds(0.3f);
            ReSetIngredientCook();
        }

        protected virtual void ReSetIngredientCook()
        {
            ReleaseIngredient();
            MoveBack();
            Rotate(_initZRotate);
            col.enabled = true;
        }
        private void ReleaseIngredient()
        {
            if (_ingredient != null && _ingredient.GetIsSnap())
            {
                _isSnap = _ingredient.GetIsSnap();
            }
            _ingredient = null;
            ingredientSprite.sprite = null;
            ingredientSprite.color = Color.white;
            spoonSprite.sprite = spoonSpriteOnNormal;
        }

        private void MoveBack()
        {
            _isPerformingAction = true;
            _actionTween?.Kill();
            onStartMoveBack?.Invoke();
            if (jumpMoveBack)
            {
                _actionTween = Tf.DOLocalMove(_initLocalPos + Vector3.up * yAddedFromLocalPos, 0.3f)
                    .OnComplete(() =>
                    {
                        SetSortingOrder(_sortingOrder);
                        _actionTween = Tf.DOLocalMove(_initLocalPos, 0.2f).OnComplete(() =>
                        {
                            if (_isSnap == true)
                            {
                                onMoveBackAfterSnap?.Invoke();
                            }
                            onEndMoveBack?.Invoke();
                            _isPerformingAction = false;
                        });
                    });
            }
            else
            {
                _actionTween = Tf.DOLocalMove(_initLocalPos, 0.3f).OnComplete(() =>
                {
                    SetSortingOrder(_sortingOrder);
                    if (_isSnap == true)
                    {
                        onMoveBackAfterSnap?.Invoke();
                    }
                    onEndMoveBack?.Invoke();
                    _isPerformingAction = false;
                });
            }
        }

        private void Rotate(float z)
        {
            _rotateTween?.Kill();
            _rotateTween = Tf.DOLocalRotate(new Vector3(0, 0, z), 0.3f);
        }

        private void SetSortingOrder(int order)
        {
            spoonSprite.sortingOrder = order;
            ingredientSprite.sortingOrder = order + 1;
            if (isSpriteFont)
            {
                if (spriteFont != null)
                {
                    spriteFont.sortingOrder = order + 2;
                }
            }
        }

        private void CheckTriggerOnPick()
        {
            Collider2D[] results = new Collider2D[10];
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            int count = col.OverlapCollider(filter, results);

            for (int i = 0; i < count; i++)
            {
                Collider2D other = results[i];
                if (_ingredient) return;

                SpoonIngredient ingredient = TryGetIngredient(other);
                if (!ingredient) continue;
                if (!acceptIngredients.Contains(ingredient)) continue;
                if (ingredient.IgnoreThis) continue;

                _ingredient = ingredient;
                ingredient.ShareIngredientFeedback();
                ingredientSprite.sprite = ingredient.Sprite.spriteWhenSpoonHold;
                SoundManager.PlaySFX(takeIngredientSound);
                break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isDragging) return;
            if (_ingredient) return;
            if (triggerToRotate.IsBlockRotate == false)
            {
                return;
            }
            else
            {
                if (isBlockTrySnap)
                {
                    emoji.ShowNegative();
                }
                else
                {
                    SpoonIngredient ingredient = TryGetIngredient(other);
                    if (!ingredient) return;
                    if (!acceptIngredients.Contains(ingredient)) return;
                    if (ingredient.IgnoreThis) return;
                    _ingredient = ingredient;
                    ingredient.ShareIngredientFeedback();
                    ingredientSprite.sprite = ingredient.Sprite.spriteWhenSpoonHold;
                    SoundManager.PlaySFX(takeIngredientSound);
                }

            }
        }

        private SpoonIngredient TryGetIngredient(Collider2D colCache)
        {
            if (_cachedIngredients.TryGetValue(colCache, out SpoonIngredient ingredient))
            {
                return ingredient;
            }
            ingredient = colCache.GetComponent<SpoonIngredient>();
            if (!ingredient) return null;
            _cachedIngredients.Add(colCache, ingredient);
            return ingredient;
        }
        public void SetScaleObj()
        {
            this.transform.localScale = Vector3.zero;
        }
    }
}