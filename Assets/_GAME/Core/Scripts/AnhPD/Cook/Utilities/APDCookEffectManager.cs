using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AnhPD.Cook
{
    public class APDCookEffectManager : MonoBehaviour
    {
        public static APDCookEffectManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        public enum EffectType
        {
            Clean = 0,
        }
        [SerializeField] private GameObject[] effects;

        public void SpawnEffect_Clean(Vector2 pos, float ration = 1f)
        {
            GameObject go = Instantiate(effects[(int)EffectType.Clean]);
            go.transform.position = pos;
            go.transform.localScale = Vector3.one * .3f * ration;
            go.SetActive(true);
        }
    }
}

