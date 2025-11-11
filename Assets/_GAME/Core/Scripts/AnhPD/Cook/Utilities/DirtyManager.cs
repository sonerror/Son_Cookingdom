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
        [Button]
        public void FindAllDirties(bool include_Inactive = true)
        {
            dirties = FindObjectsByType<DirtyClean>
                (include_Inactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude,
                FindObjectsSortMode.InstanceID);
            InitDirty();
        }
        [Button]
        private void InitDirty()
        {
            for (int i = 0; i < dirties.Length; i++)
            {
                dirties[i].Init(this);
            }
        }
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
            if(spawnIndex >= dirties.Length) return;
            dirties[spawnIndex].Appear();
            spawnIndex++;
        }
    }
}

