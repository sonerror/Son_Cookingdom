
using AnhPD.Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class HookKingCrab : Level110HookPlayer
    {
        protected override void OnRotating()
        {
            //base.OnRotating();

            angle += Time.deltaTime * dirX * rotateSpeed;
            if (angle * dirX >= angleMax)
            {
                dirX *= -1;
            }
            anchorPosition.transform.rotation = Quaternion.Euler(0, 0, angle);

            if (LevelKingCrab.Instance.IsReady && Input.GetMouseButtonDown(0))
            {
                // if (EventSystem.current.IsPointerOverUIObject()) return;
                //AudioManager.PlaySFX(sfxShot);
                ChangeState(HookState.Shoot);
            }
        }
        protected override void OnReturned()
        {
            //base.OnReturned();
            ChangeState(HookState.None);

            if (isCatching)
            {
                isCatching = false;
                fishCatched.DisableObject();
                fishFlyToBoard.gameObject.SetActive(true);

                gameObject.SetActive(false);

                capybara.OnFishReturned(() =>
                {
                    if (fishColorType != Fish.ColorType.KingCrab)
                    {
                        capybara.OnAngry();
                    }
                    else
                    {
                        capybara.OnHappy();
                        // LevelKingCrab.Instance.OnCrabCatched();
                    }

                    isDamaged = false;
                    dirX *= -1;
                });
            }
            else
            {
                ChangeState(HookState.Rotation);

                isDamaged = false;
                dirX *= -1;
            }


        }
        public override void OnBited()
        {
            isDamaged = true;
            //base.OnBited();
            gameObject.SetActive(false);

            lineRenderer.SetPosition(0, anchorPosition.position);
            lineRenderer.SetPosition(1, anchorPosition.position);

            capybara.OnDamaged();
        }
    }
}

