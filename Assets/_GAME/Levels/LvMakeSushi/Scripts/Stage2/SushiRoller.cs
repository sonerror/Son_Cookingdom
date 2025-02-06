using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class SushiRoller : MonoBehaviour
    {
        [SerializeField] private Animator rollAnim;
        [SerializeField] private AudioSource sfxRolling;
        private float rollerOffsetY = 1.03f, rate = 0.05f, rollerStartY;
        private bool isStarRolling = false, isDragging = false;

        private Vector2 startPos;

        private void Start()
        {
            rollerStartY = transform.localPosition.y;
            rollerOffsetY = 1.03f + Mathf.Abs(rollerStartY);
        }

        public void StartRolling()
        {
            float duration = .2f;
            rollAnim.transform.DOScaleY(1, duration);
            isStarRolling = true;
            transform.DOLocalMoveY(rollerStartY + (rollerOffsetY) * rate, duration);
        }

        private void OnMouseDown()
        {
            if (!isStarRolling) return;

            isDragging = true;
            startPos = (Vector2)Input.mousePosition;
        }
        private void OnMouseDrag()
        {

            Vector2 mousePos = (Vector2)Input.mousePosition;
            Vector2 currentPos = (Vector2)Camera.main.WorldToScreenPoint(transform.position);
            if (mousePos.y > currentPos.y && isDragging)
            {
                if (!rollAnim.enabled)
                {
                    rollAnim.enabled = true;
                    sfxRolling.Play();
                }
                rate += .03f;
                rate = Mathf.Clamp01(rate);
                transform.DOLocalMoveY(rollerStartY + (rollerOffsetY) * rate, .01f);

                if (rate > 0.999f)
                {
                    OnMouseUp();
                    LevelMakeSushi.Instance.OnSushiComplete(transform.position);
                }
            }
            else
            {
                rollAnim.enabled = false;
                sfxRolling.Pause();
            }
        }
        private void OnMouseUp()
        {
            rollAnim.enabled = false;
            isDragging = false;
            sfxRolling.Pause();
        }
    }
}

