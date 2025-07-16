using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShowItem : GameUnit
{
    [SerializeField] private float distance = 1f;

    public void MoveObjShow()
    {
        Vector3 targetPosition = Tf.position;
        Tf.position = Tf.position + Vector3.right * distance;
        Tf.DOMove(targetPosition, 1f);
    }

    public void MoveObjHide()
    {
        var pos = Tf.position + Vector3.left * distance;
        Tf.DOMove(pos, 1f).OnComplete(() =>
        {
            Tf.gameObject.SetActive(false);
        });
    }
}
