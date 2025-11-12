using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class GarbageManager : MonoBehaviour
  {
    [SerializeField] private List<GarbageDrag> garbages;

    public bool IsClean
    {
      get
      {
        for (int i = 0; i < garbages.Count; i++)
        {
          if (garbages[i].gameObject.activeSelf) return false;
        }
        return true;
      }
    }

    public bool IsRemoveAllGarbage()
    {
      for (int i = 0; i < garbages.Count; i++)
      {
        if (garbages[i].gameObject.activeSelf) return false;
      }
      return true;
    }
    public void AddGarbage(GarbageDrag garbage)
    {
      garbages.Add(garbage);
    }
  }
}

