using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : GameUnit
{
    private ItemMove itemMove = null;
    public ItemMove ItemMove => itemMove;
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

    public void OnActionStart()
    {
        throw new System.NotImplementedException();
    }

    public void OnActionEnd()
    {
        throw new System.NotImplementedException();
    }

    public void ActionOnAddKnife(Knife knife)
    {

        knife.PlayAnimCut(itemMove.StartPoint, itemMove.FinishPoint);
        itemMove.PlayAnimCut();

    }

    public void DoneActionCut()
    {
        itemMove = null;
    }

}
