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
        gameObject.SetActive(true);
        PoolManager.Ins.Spawn(PoolType.SfxMove, Tf.position, Quaternion.identity);
        if (actionOnTheLeft)
        {
            Vector3 targetPosition = Tf.position;
            Tf.position = Tf.position + Vector3.left * distance;
            Tf.DOMove(targetPosition, 0.5f).OnComplete(() =>
            {
                PoolManager.Ins.Spawn(PoolType.SfxMove, Tf.position, Quaternion.identity);
            });
        }
        else
        {
            Vector3 targetPosition = Tf.position;
            Tf.position = Tf.position + Vector3.right * distance;
            Tf.DOMove(targetPosition, 0.5f).OnComplete(() =>
            {
                PoolManager.Ins.Spawn(PoolType.SfxMove, Tf.position, Quaternion.identity);
            });
        }
    }

    public void MoveObjHide()
    {
        if (actionOnTheLeft)
        {
            var pos = Tf.position + Vector3.left * distance;
            Tf.DOMove(pos, 0.75f).OnComplete(() =>
            {
                Tf.gameObject.SetActive(false);
            });
        }
        else
        {
            var pos = Tf.position + Vector3.right * distance;
            Tf.DOMove(pos, 0.75f).OnComplete(() =>
            {
                Tf.gameObject.SetActive(false);
            });
        }
    }
}
