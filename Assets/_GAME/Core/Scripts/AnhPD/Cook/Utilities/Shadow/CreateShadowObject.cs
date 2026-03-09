using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class CreateShadowObject : MonoBehaviour
    {
        [Button]
        public void CreateShadown()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null || sr.sprite == null) return;

            // Tạo object con
            GameObject shadowObj = new GameObject("shadow");
            shadowObj.transform.SetParent(transform);
            shadowObj.transform.localPosition = Vector3.zero;
            shadowObj.transform.localRotation = Quaternion.identity;
            shadowObj.transform.localScale = Vector3.one;

            // Thêm SpriteRenderer
            SpriteRenderer shadowSR = shadowObj.AddComponent<SpriteRenderer>();
            shadowSR.sprite = sr.sprite;
            shadowSR.color = new Color(0, 0, 0, .25f);
            shadowSR.sortingLayerID = sr.sortingLayerID;
            shadowSR.sortingOrder = sr.sortingOrder - 1;
            shadowSR.flipX = sr.flipX;
            shadowSR.flipY = sr.flipY;
            shadowSR.transform.localPosition += new Vector3(.1f, -.1f, 0);

            // Gỡ script này khỏi GameObject
            DestroyImmediate(this);
        }
    }
}

