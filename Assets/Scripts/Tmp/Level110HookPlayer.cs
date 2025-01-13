
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.Fishing
{
    public class Level110HookPlayer : Level110Hook
    {
        [SerializeField] protected CapybaraFishing capybara;
        protected override void OnRotating()
        {
            base.OnRotating();

            if (LevelFishing.Instance.IsReady && Input.GetMouseButtonDown(0))
            {
                // if (EventSystem.current.IsPointerOverUIObject()) return;
                //AudioManager.PlaySFX(sfxShot);
                ChangeState(HookState.Shoot);
            }
        }

        protected override void OnShooting()
        {
            base.OnShooting();
            if (Input.GetMouseButtonDown(0))
            {
                Rewind();
            }
        }
        public override void OnCatched(FishableObject obj)
        {
            base.OnCatched(obj);
            capybara.OnCatched();
        }
        protected override void OnReturned()
        {
            ChangeState(HookState.None);

            if (isCatching)
            {
                isCatching = false;
                fishCatched.DisableObject();
                fishFlyToBoard.gameObject.SetActive(true);

                gameObject.SetActive(false);

                capybara.OnFishReturned(() =>
                {
                    if (!LevelFishing.Instance.CheckFish(fishColorType))
                    {
                        healBar.OnHit();
                        capybara.OnAngry();
                    }
                    else
                    {
                        if (LevelFishing.Instance.IsComplete)
                        {
                            capybara.OnHappy();
                        }
                        else
                        {
                            capybara.GetReady();
                        }
                    }
                    base.OnReturned();
                });
            }
            else
            {
                ChangeState(HookState.Rotation);
                base.OnReturned();
            }
        }

        public void SetHookSpeed(float speed)
        {
            this.speed = speed;
        }

        public virtual void OnBited()
        {
            healBar.OnHit();
            gameObject.SetActive(false);

            lineRenderer.SetPosition(0, anchorPosition.position);
            lineRenderer.SetPosition(1, anchorPosition.position);

            capybara.OnDamaged();
        }
    }
}

