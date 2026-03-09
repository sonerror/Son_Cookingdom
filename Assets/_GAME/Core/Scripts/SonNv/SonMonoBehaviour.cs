using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonMonoBehaviour : MonoBehaviour
{
    private Transform _tf;

    public Transform Tf => _tf ? _tf : _tf = transform;

    protected float DistanceToInSqr(Transform target)
    {
        return (Tf.position - target.position).sqrMagnitude;
    }

    protected float DistanceToInSqr(Vector3 target)
    {
        return (Tf.position - target).sqrMagnitude;
    }

    protected float DistanceToInSqrVec2(Transform target)
    {
        Vector2 targetPos = target.position;
        Vector2 myPos = Tf.position;
        return (myPos - targetPos).sqrMagnitude;
    }

    protected float DistanceToInSqrVec2(Vector2 point1)
    {
        Vector2 myPos = Tf.position;
        return (myPos - point1).sqrMagnitude;
    }
}
