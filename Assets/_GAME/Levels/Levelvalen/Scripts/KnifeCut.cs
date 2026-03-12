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
        [SerializeField] private List<GameObject> listKnifeObjCutSlice;
        [SerializeField] private List<ParticleSystem> listSlashVfxSlice;
        [SerializeField] private float sliceDistance = 1.2f;

        [SerializeField] private KnifeCutType typeCut;
        public void SetTypeCut(KnifeCutType _typeCut)
        {
            typeCut = _typeCut;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (cuttingBoard.CanCut)
            {
                if (_isPlaying) return;
                switch (typeCut)
                {
                    case KnifeCutType.Slice://cat lat
                        AnimKnifeSlice(listSlashVfxSlice, listKnifeObjCutSlice);
                        break;
                    case KnifeCutType.Chop://cat nhanh
                        break;
                    case KnifeCutType.Dice:// cat hat luu
                        AnimKnifeDance(listSlashVfx, listKnifeObj);
                        break;
                    case KnifeCutType.None:
                        AnimKnifeDance(listSlashVfx, listKnifeObj);
                        break;
                }
            }
        }
        public void SetDataObjectCut(CuttingObject _cuttingCut)
        {
            _currentCuttingCut = _cuttingCut;
        }
        private void AnimKnifeDance(List<ParticleSystem> _listSlashVfx, List<GameObject> _listKnifeObj)
        {
            _isPlaying = true;

            spriteRenderer.enabled = false;

            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < _listKnifeObj.Count; i++)
            {
                int index = i;

                seq.AppendCallback(() =>
                {
                    _listSlashVfx[index].Play();
                    _listKnifeObj[index].SetActive(true);
                    SoundManager.PlaySFXOneShot(sfxSlashKnife);
                });

                seq.Append(_listKnifeObj[index].transform.DOLocalRotate(
                    _listKnifeObj[index].transform.localEulerAngles + new Vector3(0, 0, 180),
                    timeDur));

                seq.AppendCallback(() =>
                {
                    _listKnifeObj[index].SetActive(false);
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
                        TutorialManager.Ins.SetIsSnapKnife(false);

                        knifeOut.MoveBack();
                    }
                }
                eventDoneActionDance?.Invoke();

                _isPlaying = false;
            });
        }

        private void AnimKnifeSlice(List<ParticleSystem> _listSlashVfx, List<GameObject> _listKnifeObj)
        {
            _isPlaying = true;

            spriteRenderer.enabled = false;

            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < _listKnifeObj.Count; i++)
            {
                int index = i;

                Transform knifeTf = _listKnifeObj[index].transform;

                Vector3 startPos = knifeTf.localPosition;
                Vector3 upPos = startPos + Vector3.up * sliceDistance;
                Vector3 downPos = startPos;

                seq.AppendCallback(() =>
                {
                    _listKnifeObj[index].SetActive(true);
                    SoundManager.PlaySFXOneShot(sfxSlashKnife);
                });

                // nhấc dao lên
                seq.Append(knifeTf.DOLocalMove(upPos, timeDur * 0.5f).SetEase(Ease.OutQuad));

                // cắt xuống
                seq.Append(knifeTf.DOLocalMove(downPos, timeDur * 0.5f).SetEase(Ease.InQuad));

                seq.AppendCallback(() =>
                {
                    _listKnifeObj[index].SetActive(false);

                    // play VFX trễ 1 dao
                    if (index - 1 >= 0 && index - 1 < _listSlashVfx.Count)
                        _listSlashVfx[index - 1].Play();

                    if (index == _listKnifeObj.Count - 1 && _listSlashVfx.Count > 0)
                        _listSlashVfx[_listSlashVfx.Count - 1].Play();
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
                        TutorialManager.Ins.SetIsSnapKnife(false);
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
            SetScale(1.2f);
        }
    }

}