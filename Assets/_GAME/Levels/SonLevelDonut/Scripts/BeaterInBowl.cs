using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class BeaterInBowl : MonoBehaviour
    {
        [SerializeField] Transform tfRoot;
        [SerializeField] SpriteRenderer mixerFlour;

        [SerializeField] private AudioClip rotateSfx;
        [SerializeField] private Phase1Donut phase1;
        Vector2 mouseStarPos;
        int count = 0;
        float deltaAngle = 0;

        private void Update()
        {
            transform.eulerAngles = Vector3.zero;
        }

        private void OnMouseDrag()
        {
            if (phase1.isRotateBeater)
            {
                Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float angle = GetAngleABC(transform.position, tfRoot.position, pos);
                float z = Mathf.Lerp(tfRoot.eulerAngles.z, tfRoot.eulerAngles.z + angle, Time.deltaTime * 5f);
                deltaAngle += Mathf.Abs(z - tfRoot.eulerAngles.z);
                tfRoot.eulerAngles = new Vector3(0, 0, z);
                if (deltaAngle >= 360f)
                {
                    count += 2;
                    //.PlaySFX(rotateSfx);
                    deltaAngle %= 360f;
                    if (count >= 10)
                    {
                        phase1.CheckDoneStep3();
                    }
                }
                mixerFlour.SetAlpha(count * 0.1f + (deltaAngle / 360f) * 0.2f);
            }
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
