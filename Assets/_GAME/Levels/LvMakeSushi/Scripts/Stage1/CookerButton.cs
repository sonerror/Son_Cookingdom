using DG.Tweening;
using Satisgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class CookerButton : MonoBehaviour
    {
        [SerializeField] GameObject red, green;
        [SerializeField] AudioClip sfxClick;
        private bool isCanTouch = false;
        private bool isStartCooking = false;
        private float timer = 0f;

        [SerializeField] ParticleSystem fxSmoke;
        [SerializeField] Level127DraggableObject lid;
        [SerializeField] BoxCollider lidBlock;
        [SerializeField] AudioSource sfxBoid;

        [SerializeField] GameObject riceCooked, rice;
        [SerializeField] EmojiControl emoji;

        public bool IsCooked = false;

        private void OnMouseDown()
        {
            // if (!LevelBase.instance.IsAllowInteract || !isCanTouch) return;

            LevelMakeSushi.Instance.OnStartCooking();

            red.SetActive(true);
            green.SetActive(false);
            // AudioManager.PlaySfx(sfxClick);
            fxSmoke.gameObject.SetActive(true);
            lid.transform.DOShakeRotation(.5f, 2, 15).OnComplete(() =>
            {
                lid.transform.eulerAngles = Vector3.zero;
            }).SetLoops(-1, LoopType.Restart);
            sfxBoid.Play();

            isStartCooking = true;
            isCanTouch = false;

            emoji.ShowPositive();
        }

        private void Update()
        {
            if (!isStartCooking || IsCooked) return;
            timer += Time.deltaTime;
            if (timer > 10f)
            {
                IsCooked = true;
                LevelMakeSushi.Instance.CheckCompleteStage1();
            }
        }

        public void OnComplete()
        {
            lid.endDragEvents.RemoveAllListeners();
            lid.isRewind = false;

            // AudioManager.PlaySfx(sfxClick);
            red.SetActive(false);
            green.SetActive(true);
            fxSmoke.Stop();
            sfxBoid.Stop();
            lid.transform.DOComplete();
            lid.transform.DOKill();
            lid.UnlockPosition();
            lid.endDragEvents.AddListener(removeBarrier);
            void removeBarrier()
            {
                lidBlock.enabled = false;
            }

            rice.SetActive(false);
            riceCooked.SetActive(true);

            Destroy(this);
        }
        public void OnCanTouch()
        {
            isCanTouch = true;
        }
    }
}

