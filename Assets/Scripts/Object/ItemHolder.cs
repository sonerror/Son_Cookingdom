using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemHolder : GameUnit
{
    private ItemMove itemMove = null;
    public ItemMove ItemMove => itemMove;
    private Knife knife = null;
    public Knife Knife => knife;
    public bool IsKnife => knife != null;
    public bool IsOccupied => itemMove != null;
    [SerializeField] private Collider col;
    public Collider Col => col ? col : col = GetComponent<Collider>();


    public void AddItem(ItemMove item)
    {
        itemMove = item;
    }

    public void ActionOnAddItem()
    {

    }

    public void ActionOnAddKnife()
    {
        if (itemMove != null)
        {
            itemMove.PlayAnimCut();
        }
    }

    private bool enableCountCut = false;
    public void ActionOnAddKnife(Knife knife)
    {

        knife.PlayAnimCut(itemMove.StartPoint, itemMove.FinishPoint);
        itemMove.PlayAnimCut();
        this.knife = knife;
        resetCountCut();
        DOVirtual.DelayedCall(1.1f, () => { enableCountCut = true; });
    }

    private int countCut = 3;
    private int currCountCut = 0;

    void resetCountCut()
    {
        currCountCut = 0;
        countCut = 0;
        isPlaying = false;
    }

    void OnMouseDown()
    {

        if (!enableCountCut) return;
        if (knife != null && itemMove != null)
        {
            countCut++;
            if (countCut > 3) countCut = 3;
            PlayAnim();
        }
        TutorialManager.Ins.MouseDownItem();
    }

    void OnMouseUp()
    {
        TutorialManager.Ins.MouseUpItem();
    }

    bool isPlaying = false;
    public bool IsPlaying => isPlaying;
    void PlayAnim()
    {
        if (isPlaying) return;
        isPlaying = true;
        PlayAnimLoop();
    }

    void PlayAnimLoop()
    {
        if (currCountCut >= countCut)
        {
            isPlaying = false;
            if (currCountCut >= 3 && countCut >= 3)
            {
                DoneActionCut();
            }
            return;
        }
        EventManager.TriggerEvent(EventType.IncreaseProgressScaled.ToString());
        currCountCut++;
        knife.PlayAnimCutClick(currCountCut);
        itemMove.PlayAnimCutClick(currCountCut);
        DOVirtual.DelayedCall(0.33f, () =>
        {
            PlayAnimLoop();
        });
    }



    public void DoneActionCut()
    {
        itemMove.DoneActionCut();
        knife.DoneActionCut();
        itemMove = null;
        knife = null;
    }
}
