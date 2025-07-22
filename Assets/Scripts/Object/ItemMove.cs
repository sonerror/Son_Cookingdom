using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sonnv;
using UnityEngine;

public class ItemMove : DragController
{
    private bool isDone = false;
    public bool IsDone { get => isDone; }
    [SerializeField] private ItemHolder itemTarget;
    public Vector3 GetTargetPosition => itemTarget ? itemTarget.Tf.position : Vector3.zero;


    public bool isTofu = false;
    public ParticleSystem parState1;
    public ParticleSystem parState2;
    [SerializeField] GameObject State0;
    [SerializeField] GameObject State1;
    [SerializeField] GameObject State2;
    [SerializeField] GameObject State3;

    public Transform StartPoint;
    public Transform FinishPoint;

    // [SerializeField] private Transform mask1;
    [SerializeField] private Transform mask2;

    protected override void ActionOnMouseUp()
    {
        if (enabled)
        {
            OnDragStop();
            if (itemTarget != null && !itemTarget.IsOccupied)
            {
                if (Col.bounds.Intersects(itemTarget.Col.bounds))
                {
                    Col.enabled = false;

                    itemTarget.AddItem(this);
                    var time = Vector3.Distance(Tf.position, itemTarget.Tf.position) / 3f;
                    Tf.DOMove(itemTarget.Tf.position, time)
                        .OnComplete(() =>
                        {
                            level.PlayEmojiHeart();
                            itemTarget.ActionOnAddItem();
                            SoundManager.Ins.PlayFx(FxType.PlaceBroad);
                            EventManager.TriggerEvent(EventType.IncreaseProgress.ToString());
                        });
                    return;
                }
            }

            if (moveBackOnRelease)
            {
                Debug.Log("Move back on release");
                StartRelease();
            }

        }
        // TutorialManager.Ins.MouseUpItem();
    }

    public void PlayAnimCut()
    {
        StartCoroutine(IE_PlayAnimCut());
    }

    IEnumerator IE_PlayAnimCut()
    {
        yield return Cache.GetWFS(0.25f);
        parState1.Play();
        yield return Cache.GetWFS(0.75f);
        State0.SetActive(false);
        State1.SetActive(true);
        mask2.gameObject.SetActive(true);
        State2.SetActive(true);
    }

    public void PlayAnimCutClick(int index)
    {
        var tarPos = ((FinishPoint.position - StartPoint.position) * index / 3) + StartPoint.position;
        mask2.DOMove(tarPos, 0.3f);
    }

    public void DoneActionCut()
    {
        StartCoroutine(IE_DoneActionCut());

    }

    IEnumerator IE_DoneActionCut()
    {
        State1.SetActive(false);
        mask2.gameObject.SetActive(false);
        yield return Cache.GetWFS(0.1f);
        level.PlayEmojiHeart();
        yield return Cache.GetWFS(0.5f);
        StartRelease();
        EventManager.TriggerEvent(EventType.IncreaseProgress.ToString());
        if (isTofu)
        {
            yield return Cache.GetWFS(0.3f);
            State3.SetActive(true);
            State2.SetActive(false);
        }

        yield return Cache.GetWFS(0.5f);
        isDone = true;
        level.OnItemState0Done();
    }
}
