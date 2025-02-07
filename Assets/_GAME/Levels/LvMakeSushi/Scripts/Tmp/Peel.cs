using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Peel : MonoBehaviour
{
    public DraggableObject draggable;
    public float distaceCheck = 1f;
    public Transform target;


    public void CheckToTarget()
    {
        if (Vector3.Distance(transform.position, target.position) < distaceCheck)
        {
            draggable.enabled = false;
            transform.DOMove(target.position, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }

}
