using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShowItem : GameUnit
{
    [SerializeField] private float distance = 1f;
    [SerializeField] private bool actionOnTheLeft = true;

    public void MoveObjShow()
    {

        if (actionOnTheLeft)
        {
            Vector3 targetPosition = Tf.position;
            Tf.position = Tf.position + Vector3.left * distance;
            Tf.DOMove(targetPosition, 1f);
        }
        else
        {
            Vector3 targetPosition = Tf.position;
            Tf.position = Tf.position + Vector3.right * distance;
            Tf.DOMove(targetPosition, 1f);
        }
    }

    public void MoveObjHide()
    {
        if (actionOnTheLeft)
        {
            var pos = Tf.position + Vector3.left * distance;
            Tf.DOMove(pos, 1f).OnComplete(() =>
            {
                Tf.gameObject.SetActive(false);
            });
        }
        else
        {
            var pos = Tf.position + Vector3.right * distance;
            Tf.DOMove(pos, 1f).OnComplete(() =>
            {
                Tf.gameObject.SetActive(false);
            });
        }
    }
}
