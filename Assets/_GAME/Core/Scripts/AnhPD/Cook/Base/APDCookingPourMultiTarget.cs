using System.Collections;
using System.Collections.Generic;
using AnhPD.Cook;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
    public class APDCookingPourMultiTarget : APDCookingPour
    {
        [SerializeField] private PourTarget[] pourTargets;

        public void SetupForTarget(int index)
        {
            target = pourTargets[index].transform;
            eventComplete.RemoveAllListeners();
            eventComplete.AddListener(() =>
            {
                pourTargets[index].onComplete?.Invoke();
            });
        }
    }

    [System.Serializable]
    public class PourTarget
    {
        public Transform transform;
        public UnityEvent onComplete;
    }
}
