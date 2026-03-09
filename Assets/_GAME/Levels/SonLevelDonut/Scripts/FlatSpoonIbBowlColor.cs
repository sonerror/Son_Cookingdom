using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    [Serializable]
    public class IngredientRotate
    {
        public IngredientType ingredientType;
        public SpriteRenderer sprite;
    }
    [Serializable]
    public class IngredientEnd
    {
        public IngredientType ingredientType;
        public Sprite sprite;
    }
    public class FlatSpoonIbBowlColor : MonoBehaviour
    {
        [SerializeField] Transform tfRoot;
        [SerializeField] List<IngredientRotate> listLixerFlour;
        public SpriteRenderer mixerFlour;
        [SerializeField] private SpriteRenderer spatula;
        [SerializeField] IngredientHolderColor ingredientHolderColor;
        [SerializeField] IngredientForwarder ingredientForwarder;
        [SerializeField] private AudioClip rotateSfx;
        // [SerializeField] private Phase4Donut phase4;
        [SerializeField] private Collider2D col;

        Vector2 mouseStarPos;
        int count = 0;
        float deltaAngle = 0;
        public void SetSpriteRenderer(IngredientType type)
        {
            mixerFlour = listLixerFlour.Find(t => t.ingredientType == type).sprite;
        }
        private void Update()
        {
            transform.eulerAngles = new Vector3(0, 0, 2);
        }
        public void SetAlpha(float index)
        {
            spatula.SetAlpha(index);

        }
        public void HideCol(bool value)
        {
            col.enabled = value;

        }


        private float GetAngleABC(Vector2 pointA, Vector2 pointB, Vector2 pointC)
        {
            Vector2 BA = pointA - pointB;
            Vector2 BC = pointC - pointB;

            float cosTheta = Vector2.Dot(BA.normalized, BC.normalized);

            float angleRad = Mathf.Acos(Mathf.Clamp(cosTheta, -1f, 1f));

            float angleDeg = angleRad * Mathf.Rad2Deg;

            float crossZ = BA.x * BC.y - BA.y * BC.x;
            if (crossZ < 0)
            {
                angleDeg = -angleDeg;
            }

            return angleDeg;
        }
    }
}
