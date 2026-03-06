using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace sonnv
{
    public class KnifeCut : SonMonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private CuttingBoard cuttingBoard;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SonSnapObject knifeOut;

        [Header("Knife Dance")]
        [SerializeField] private List<GameObject> listKnifeObj;
        [SerializeField] private List<ParticleSystem> listSlashVfx;
        [SerializeField] private AudioClip sfxSlashKnife;
        [SerializeField] private float timeDur = 0.25f;

        [SerializeField] private CuttingObject _currentCuttingCut;

        public UnityEvent eventDoneActionDance;

        private bool _isPlaying;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (cuttingBoard.CanCut)
            {
                if (_isPlaying) return;
                AnimKnifeDance();
            }
        }
        public void SetDataObjectCut(CuttingObject _cuttingCut)
        {
            _currentCuttingCut = _cuttingCut;
        }
        private void AnimKnifeDance()
        {
            _isPlaying = true;

            spriteRenderer.enabled = false;

            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < listKnifeObj.Count; i++)
            {
                int index = i;

                seq.AppendCallback(() =>
                {
                    listSlashVfx[index].Play();
                    listKnifeObj[index].SetActive(true);
                    SoundManager.PlaySFXOneShot(sfxSlashKnife);
                });

                seq.Append(listKnifeObj[index].transform.DOLocalRotate(
                    listKnifeObj[index].transform.localEulerAngles + new Vector3(0, 0, 180),
                    timeDur));

                seq.AppendCallback(() =>
                {
                    listKnifeObj[index].SetActive(false);
                });
            }

            seq.AppendCallback(() =>
            {
                spriteRenderer.enabled = true;
                if (_currentCuttingCut != null)
                {
                    _currentCuttingCut.ActionCutDone();
                    SetScale(0);
                    if (knifeOut != null)
                    {
                        knifeOut.MoveBack();
                    }
                }
                eventDoneActionDance?.Invoke();
                _isPlaying = false;
            });
        }
        private void SetScale(float scaleAffterSnap)
        {
            Tf.localScale = Vector3.one * scaleAffterSnap;
        }
        public void ResetScale()
        {
            SetScale(1);
        }
    }

}