using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class DirtyManager : MonoBehaviour
  {
    [SerializeField] private DirtyClean[] dirties;
    public UnityEvent eventDirtyClean;
    public DirtyClean[] Dirties => dirties;
    public bool IsClean => IsRemoveAllDirty();
    public bool IsRemoveAllDirty()
    {
      for (int i = 0; i < dirties.Length; i++)
      {
        if (dirties[i].gameObject.activeSelf) return false;
      }
      return true;
    }
    public void OnDirtyClean()
    {
      eventDirtyClean?.Invoke();
    }

    private int spawnIndex;
    public void SpawnDirty()
    {
      if (spawnIndex >= dirties.Length) return;
      dirties[spawnIndex].Appear();
      spawnIndex++;
    }
  }
}

