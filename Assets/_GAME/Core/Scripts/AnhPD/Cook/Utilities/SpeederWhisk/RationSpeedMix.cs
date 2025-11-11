using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.Cook
{
    public class RationSpeedMix : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BeaterInBowl whisk;
        [SerializeField] private float ration = 2f;

        [Button]
        private void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            whisk = transform.parent.GetComponentInChildren<BeaterInBowl>();
        }
        public void OnMixing()
        {
            spriteRenderer.SetAlpha(ration * whisk.Rate);
        }
    }
}
