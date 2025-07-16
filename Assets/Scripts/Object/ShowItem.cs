using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItem : GameUnit
{
    [SerializeField] private bool fromRight = false;
    [SerializeField] private float distance = 1f;

    void Start()
    {
        MoveObj();
    }

    private void MoveObj()
    {
        Vector3 targetPosition = transform.position;
        transform.position = fromRight ? tf.position + Vector3.right * distance : tf.position + Vector3.right * distance;
        transform.localPosition = targetPosition;
    }
}
