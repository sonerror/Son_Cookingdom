using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace sonnv
{
    public class FlatSpoonInPan : SonMonoBehaviour, IDragHandler
    {
        [SerializeField] Transform tfRoot;
        [SerializeField] SpriteRenderer mixerFlour;
        [SerializeField] List<SpriteRenderer> imgList;

        [SerializeField] private AudioClip rotateSfx;
        [SerializeField] private int sovongquay = 5;
        [SerializeField] private float speedRotate = 10f;

        [SerializeField] private bool isFlipSprite = false;
        [SerializeField] private ParticleSystem burningParticles;

        public UnityEvent onEndRotate;
        public UnityEvent onDoneOne;
        private bool isBlockRotate = false;

        int count = 0;
        float deltaAngle = 0;

        private void Update()
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        public void SetBlockRotate(bool value)
        {
            isBlockRotate = value;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (isBlockRotate) return;
            float a = 2f;
            float b = 1f;
            Vector2 center = tfRoot.position;
            Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mouseWorldPos - center;
            float angleElip = Mathf.Atan2(dir.y / b, dir.x / a);
            Vector2 constrainedPos = new Vector2(a * Mathf.Cos(angleElip), b * Mathf.Sin(angleElip)) + center;
            float angle = GetAngleABC(transform.position, tfRoot.position, constrainedPos);
            float z = Mathf.Lerp(tfRoot.eulerAngles.z, tfRoot.eulerAngles.z + angle, Time.deltaTime * speedRotate);
            deltaAngle += Mathf.Abs(z - tfRoot.eulerAngles.z);
            tfRoot.eulerAngles = new Vector3(0, 0, z);

            if (deltaAngle >= 360f)
            {

                count++;
                deltaAngle %= 360f;
                SoundManager.PlaySFX(rotateSfx);
                if (isFlipSprite)
                {
                    mixerFlour.flipX = !mixerFlour.flipX;
                    StartParticles();
                }
                onDoneOne?.Invoke();
                if (count >= sovongquay)
                {
                    onEndRotate?.Invoke();
                    if (isFlipSprite)
                    {
                        StopParticles();
                    }
                }
            }
            float progress = (count + deltaAngle / 360f) / sovongquay;
            progress = Mathf.Clamp01(progress);
            float alphaMain = Mathf.SmoothStep(0f, 1f, progress);
            float alphaOthers = Mathf.SmoothStep(1f, 0f, progress);
            mixerFlour.SetAlpha(alphaMain);
            foreach (SpriteRenderer img in imgList)
            {
                img.SetAlpha(alphaOthers);

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
        private void StartParticles()
        {
            if (burningParticles != null)
            {
                burningParticles.gameObject.SetActive(true);
                burningParticles.Play();
            }
        }

        private void StopParticles()
        {
            if (burningParticles != null)
            {
                burningParticles.gameObject.SetActive(false);
                burningParticles.Stop();
            }
        }
    }
}