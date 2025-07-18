using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    bool IsDone { get; }
    Collider Col { get; }

    void OnActionStart();
    void OnActionEnd();
}
