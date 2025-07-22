using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sonnv;
using UnityEngine;

public class Knife : DragController
{
    public bool IsDone = false;
    [SerializeField] private ItemHolder itemTarget;
    [SerializeField] private Animator anim;

    protected override void ActionOnMouseUp()
    {
        if (enabled)
        {

            if (!Level630.haveKnife && itemTarget != null && itemTarget.IsOccupied)
            {
                if (Col.bounds.Intersects(itemTarget.Col.bounds))
                {
                    Level630.haveKnife = true;
                    Col.enabled = false;
                    var time = Vector3.Distance(Tf.position, itemTarget.ItemMove.StartPoint.position + Vector3.right * 0.5f) / 3f;
                    Tf.DOMove(itemTarget.ItemMove.StartPoint.position + Vector3.right * 0.3f, time)
                        .OnComplete(() =>
                        {
                            itemTarget.ActionOnAddKnife(this);
                        });
                    _dragging = false;
                    return;
                }
            }
            OnDragStop();
            if (moveBackOnRelease)
            {
                Debug.Log("Move back on release");
                StartRelease();
            }

        }
        // TutorialManager.Ins.MouseUpItem();
    }
    private Vector3 startPoint;
    private Vector3 finishPoint;
    public void PlayAnimCut(Transform startPoint, Transform finishPoint)
    {
        this.startPoint = startPoint.position + Vector3.right * 0.3f;
        this.finishPoint = finishPoint.position + Vector3.right * 0.3f;
        StartCoroutine(IE_PlayAnimCut());
    }

    IEnumerator IE_PlayAnimCut()
    {
        yield return Cache.GetWFS(0.25f);
        anim.SetTrigger("tomatocut");
        for (int i = 0; i < 3; i++)
        {
            PoolManager.Ins.Spawn(PoolType.SfxCut2, Tf.position, Quaternion.identity);
            yield return Cache.GetWFS(0.15f);
        }
        yield return Cache.GetWFS(0.3f);
    }


    public void PlayAnimCutClick(int index)
    {
        var targetPos = ((finishPoint - startPoint) * index / 3) + startPoint;
        Tf.DOMove(targetPos, 0.3f);
        var par = itemTarget.ItemMove.parState2;
        anim.SetTrigger("cut");
        if (par != null) par.Play();
        PoolManager.Ins.Spawn(PoolType.SfxCut, Tf.position, Quaternion.identity);
    }

    public void DoneActionCut()
    {
        StartCoroutine(IE_DoneActionCut());

    }

    IEnumerator IE_DoneActionCut()
    {
        yield return Cache.GetWFS(0.1f);
        yield return Cache.GetWFS(0.5f);
        StartRelease();
        Col.enabled = true;
        Level630.haveKnife = false;
    }


}
