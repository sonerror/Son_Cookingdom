using AnhPD.Fishing;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


namespace AnhPD.Fishing
{
    public class Level110Hook : MonoBehaviour
    {
        public enum HookState
        {
            None = -1,
            Rotation = 0,
            Shoot = 1,
            Rewind = 2,
        }
        [SerializeField] protected LineRenderer lineRenderer;
        [SerializeField] protected Transform anchorPosition;
        [SerializeField] protected Level110FishCatched fishCatched, fishFlyToBoard;
        [SerializeField] protected CatHeal healBar;

        [SerializeField] protected AudioClip sfxShot, sfxCatch;

        public Fish.ColorType fishColorType => fishCatched.FishColor;

        protected float speed = 15f;
        protected float rotateSpeed = 40f;
        protected float angleMax = 50f;
        private float wireLength = 5f;

        protected int dirX = 1, dirY = -1;
        protected float angle = 0f;

        protected HookState state;

        Vector2 viewPoint = Vector2.zero;

        Vector2 viewPointInCameraX = new Vector2(0.005f, 1f);
        Vector2 viewPointInCameraY = new Vector2(0.05f, 0.95f);

        private bool IsInCamera => viewPoint.x > viewPointInCameraX.x
            && viewPoint.x < viewPointInCameraX.y
            && viewPoint.y > viewPointInCameraY.x
            && viewPoint.y < viewPointInCameraY.y;

        protected bool isCatching = false;
        protected bool isDamaged = false;
        public bool IsCatching => isCatching;
        public bool IsCanCatch => state != HookState.Rotation && !isDamaged && !isCatching;
        protected bool IsAllowInteract;
        private void Start()
        {
            // IsAllowInteract = LevelBase.instance ? LevelBase.instance.IsAllowInteract : 
            //     (LevelPetBase.instance ? LevelPetBase.instance.IsAllowInteract : false);
        }

        public virtual void OnInit()
        {
            fishCatched.OnInit();
            wireLength = 5f;

            anchorPosition.eulerAngles = Vector3.zero;
            angle = 0f;

            transform.localPosition = new Vector2(0, -1f);
            transform.DOLocalMoveY(-5f, .3f).OnComplete(() =>
            {
                ChangeState(HookState.Rotation);
            });
        }

        private void Update()
        {
            lineRenderer.SetPosition(0, anchorPosition.position);
            lineRenderer.SetPosition(1, transform.position);

            if (!IsAllowInteract) return;

            switch (state)
            {
                case HookState.Rotation:

                    OnRotating();

                    break;
                case HookState.Shoot:

                    OnShooting();

                    break;
                case HookState.Rewind:
                    if (transform.localPosition.y >= -wireLength)
                    {
                        OnReturned();
                    }
                    else
                    {
                        OnRewinding();
                    }
                    break;
            }
        }
        protected virtual void OnRotating()
        {
            angle += Time.deltaTime * dirX * rotateSpeed;
            if (angle * dirX >= angleMax)
            {
                dirX *= -1;
            }
            anchorPosition.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        protected virtual void OnShooting()
        {
            viewPoint = Camera.main.WorldToViewportPoint(transform.position);

            dirY = -1;

            transform.localPosition += Vector3.up * (speed * Time.deltaTime) * dirY;

            if (!IsInCamera)
            {
                Rewind();
            }
        }
        protected virtual void OnReturned()
        {
            isDamaged = false;

            dirX *= -1;
        }

        protected virtual void Rewind()
        {
            dirY = 1;
            ChangeState(HookState.Rewind);
        }
        protected virtual void OnRewinding()
        {
            transform.localPosition += Vector3.up * (speed * Time.deltaTime) * dirY;
        }

        public void OnBlocked()
        {
            dirY = 1;
            ChangeState(HookState.Rewind);
        }

        public virtual void OnCatched(FishableObject obj)
        {
            wireLength = 1f;
            fishCatched.EnableObject();

            switch (obj.type)
            {
                case FishableObject.Type.Fish:
                    fishCatched.OnInitFishSprite(((Fish)obj).Color);
                    fishFlyToBoard.OnInitFishSprite(((Fish)obj).Color);
                    break;
                case FishableObject.Type.Garbage:
                    fishCatched.OnInitGarbageSprite(((Garbage)obj).gType);
                    fishFlyToBoard.OnInitGarbageSprite(((Garbage)obj).gType);
                    break;
            }

            ChangeState(HookState.Rewind);
            Rewind();

            //AudioManager.PlaySFX(sfxCatch);

            isCatching = true;
        }
        public void OnDamaged()
        {
            isDamaged = true;
            healBar.OnHit();
            Rewind();
        }
        public void OnHeal()
        {
            healBar.OnHeal();
        }
        public void ChangeState(HookState newState)
        {
            if (state != newState)
            {
                state = newState;
            }
        }
    }
}

