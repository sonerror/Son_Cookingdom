using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace sonnv
{
    public class TapProgressBar : MonoBehaviour
    {
        [SerializeField] private Transform checkPointArrow;

        [SerializeField] private float maxX;
        [SerializeField] private float minX;
        [SerializeField] private float speed;

        [SerializeField] private float timeToMove;

        [SerializeField] private List<CheckPointDetail> listCheckPointDetail;

        [SerializeField] private List<ParticleSystem> correctEffect;
        [SerializeField] private List<ParticleSystem> wrongEffect;

        [SerializeField] private AudioSource correctSound;
        [SerializeField] private AudioSource wrongSound;

        private int indexListCheckPoint = 0;
        private int countCheckPoint = 0;

        private bool isPauseArrow = false;
        private bool isDoneTurnPlay = false;

        private bool isStart = false;
        private bool isCanTap = false;

        [Button]
        protected virtual void InitProperties()
        {
            foreach (var checkPointDetail in listCheckPointDetail)
            {
                foreach (var checkPoint in checkPointDetail.checkPoints)
                {
                    checkPoint.spriteRenderer =
                        checkPoint.checkPoint.GetComponentInChildren<SpriteRenderer>();
                }
            }
        }

        private void Update()
        {
            if (isDoneTurnPlay) return;
            if (isPauseArrow) return;

            Vector3 pos = checkPointArrow.localPosition;
            pos.x += speed * Time.deltaTime;

            if (pos.x >= maxX)
                pos.x = minX;

            checkPointArrow.localPosition = pos;
        }

        public void EventStart()
        {
            if (isStart) return;

            isStart = true;
            InitFirstCheckPoint();
        }

        public void EventTap()
        {
            if (isDoneTurnPlay || !isStart || !isCanTap) return;

            transform.DOPunchScale(Vector3.one * .05f, .3f);

            countCheckPoint++;

            CheckPointArrowIsInRange();
        }

        public void SetTimeToMove(float time)
        {
            timeToMove = time;
        }

        private void InitFirstCheckPoint()
        {
            foreach (var checkPoint in listCheckPointDetail[indexListCheckPoint].checkPoints)
            {
                checkPoint.spriteRenderer.SetAlpha(0);
                checkPoint.isDone = false;
            }

            speed = listCheckPointDetail[indexListCheckPoint].speed;

            transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                foreach (var checkPoint in listCheckPointDetail[indexListCheckPoint].checkPoints)
                {
                    checkPoint.spriteRenderer.DOFade(1, 0.3f).OnComplete(() =>
                    {
                        isDoneTurnPlay = false;
                        isPauseArrow = false;
                        countCheckPoint = 0;
                        isCanTap = true;
                    });
                }
            });
        }

        private void CheckPointArrowIsInRange()
        {
            bool isCorrect = false;

            foreach (var checkPoint in listCheckPointDetail[indexListCheckPoint].checkPoints)
            {
                if (checkPoint.isDone) continue;

                if (checkPointArrow.localPosition.x >=
                    checkPoint.checkPoint.localPosition.x - 0.035f &&
                    checkPointArrow.localPosition.x <=
                    checkPoint.checkPoint.localPosition.x + 0.035f)
                {
                    checkPoint.isDone = true;

                    checkPoint.spriteRenderer.DOFade(0, 0.3f);

                    foreach (var effect in correctEffect)
                    {
                        effect.transform.position =
                            new Vector3(checkPointArrow.position.x,
                            effect.transform.position.y,
                            effect.transform.position.z);

                        effect.Play();
                    }

                    correctSound.Play();

                    isCorrect = true;

                    break;
                }
            }

            if (!isCorrect)
            {
                isCanTap = false;
                isDoneTurnPlay = true;
                isPauseArrow = true;

                foreach (var effect in wrongEffect)
                {
                    effect.transform.position =
                        new Vector3(checkPointArrow.position.x,
                        effect.transform.position.y,
                        effect.transform.position.z);

                    effect.Play();
                }

                if (speed >= 0.3f)
                    speed -= 0.2f;

                wrongSound.Play();

                listCheckPointDetail[indexListCheckPoint]
                    .eventWhenFailCheckPoints?.Invoke();

                StartCoroutine(ResetAll());
            }

            if (countCheckPoint ==
                listCheckPointDetail[indexListCheckPoint].checkPoints.Count)
            {
                isCanTap = false;
                isDoneTurnPlay = true;
                isPauseArrow = true;

                foreach (var checkPoint in listCheckPointDetail[indexListCheckPoint].checkPoints)
                {
                    if (!checkPoint.isDone)
                    {
                        foreach (var effect in wrongEffect)
                        {
                            effect.transform.localPosition =
                                new Vector3(checkPointArrow.localPosition.x,
                                effect.transform.localPosition.y,
                                effect.transform.localPosition.z);

                            effect.Play();
                        }

                        wrongSound.Play();

                        listCheckPointDetail[indexListCheckPoint]
                            .eventWhenFailCheckPoints?.Invoke();

                        StartCoroutine(ResetAll());

                        return;
                    }
                }

                if (indexListCheckPoint < listCheckPointDetail.Count - 1)
                {
                    listCheckPointDetail[indexListCheckPoint]
                        .eventWhenCompleteCheckPoints?.Invoke();

                    StartCoroutine(InitNextCheckPoint());
                    return;
                }
                else
                {
                    listCheckPointDetail[indexListCheckPoint]
                        .eventWhenCompleteCheckPoints?.Invoke();

                    StartCoroutine(CloseAll());
                }
            }
        }

        private IEnumerator ResetAll()
        {
            yield return new WaitForSeconds(2f);

            transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                checkPointArrow.localPosition =
                    new Vector3(minX,
                    checkPointArrow.localPosition.y,
                    checkPointArrow.localPosition.z);

                foreach (var checkPoint in
                    listCheckPointDetail[indexListCheckPoint].checkPoints)
                {
                    checkPoint.spriteRenderer.SetAlpha(0);
                    checkPoint.isDone = false;
                }
            });

            yield return new WaitForSeconds(0.5f);

            transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                foreach (var checkPoint in
                    listCheckPointDetail[indexListCheckPoint].checkPoints)
                {
                    checkPoint.spriteRenderer.DOFade(1, 0.3f);
                }
            });

            yield return new WaitForSeconds(0.6f);

            isCanTap = true;
            isDoneTurnPlay = false;
            isPauseArrow = false;
            countCheckPoint = 0;
        }

        private IEnumerator InitNextCheckPoint()
        {
            foreach (var checkPoint in listCheckPointDetail[indexListCheckPoint].checkPoints)
                checkPoint.spriteRenderer.DOFade(0, 0.3f);

            yield return new WaitForSeconds(0.3f);

            listCheckPointDetail[indexListCheckPoint].root.gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);

            transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                indexListCheckPoint++;

                speed = listCheckPointDetail[indexListCheckPoint].speed;

                listCheckPointDetail[indexListCheckPoint].root.gameObject.SetActive(true);

                checkPointArrow.localPosition =
                    new Vector3(minX,
                    checkPointArrow.localPosition.y,
                    checkPointArrow.localPosition.z);

                foreach (var checkPoint in listCheckPointDetail[indexListCheckPoint].checkPoints)
                {
                    checkPoint.checkPoint.gameObject.SetActive(true);
                    checkPoint.spriteRenderer.SetAlpha(0);
                }
            });

            yield return new WaitForSeconds(0.5f);

            transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                foreach (var checkPoint in listCheckPointDetail[indexListCheckPoint].checkPoints)
                {
                    checkPoint.isDone = false;

                    checkPoint.spriteRenderer.DOFade(1, 0.3f).OnComplete(() =>
                    {
                        isCanTap = true;
                        isDoneTurnPlay = false;
                        isPauseArrow = false;
                        countCheckPoint = 0;
                    });
                }
            });
        }

        [SerializeField] private UnityEvent eventCompleteAll;
        public UnityEvent EventCompleteAll => eventCompleteAll;

        private IEnumerator CloseAll()
        {
            yield return new WaitForSeconds(2f);

            eventCompleteAll?.Invoke();

            transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}