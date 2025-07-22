using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sonnv;
using UnityEngine;

public class ItemMoveState2 : DragController
{
    private bool isDone = false;
    public bool IsDone { get => isDone; }

    public bool IsEnableCheck = false;
    [SerializeField] private ItemHolder itemTarget;
    public Vector3 GetTargetPosition => itemTarget ? itemTarget.Tf.position : Vector3.zero;
    [SerializeField] private FxType fxDone = FxType.None;


    protected override void ActionOnMouseUp()
    {
        if (enabled)
        {
            OnDragStop();
            if (IsEnableCheck && itemTarget != null && !itemTarget.IsOccupied)
            {
                if (Col.bounds.Intersects(level.stoveCollider.bounds))
                {
                    Col.enabled = false;

                    var time = Vector3.Distance(Tf.position, itemTarget.Tf.position) / 3f;
                    Tf.DOMove(itemTarget.Tf.position, time)
                        .OnComplete(() =>
                        {
                            itemTarget.gameObject.SetActive(true);
                            gameObject.SetActive(false);
                            level.NextStepInState2();
                            SoundManager.Ins.PlayFx(fxDone);
                            EventManager.TriggerEvent(EventType.IncreaseProgress.ToString());
                        });
                    return;
                }
            }

            if (moveBackOnRelease)
            {
                StartRelease();
            }

        }
        // TutorialManager.Ins.MouseUpItem();
    }


}
