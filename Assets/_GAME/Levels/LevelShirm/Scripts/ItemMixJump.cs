using DG.Tweening;
using Sirenix.OdinInspector;
using System; // Thêm thư viện này để dùng Action
using System.Collections;
using UnityEngine;

namespace sonnv
{
    public class ItemMixJump : MonoBehaviour
    {
        public Action OnItemDoneEvent;

        [SerializeField] private Vector3 center;
        [SerializeField] private float radius;
        [SerializeField] float jumpower = 1;
        [SerializeField] float duration = 0.3f;
        [SerializeField] private SpriteRenderer rawSR, doneSR;
        [SerializeField] Collider2D col;

        [Header("Cooking Settings")]
        [SerializeField] private float cookStep = 0.25f; // Mỗi lần nảy sẽ tăng 25% độ chín (cần 4 lần nảy)

        public Transform TF => transform;
        public bool IsDone => isDone; // Mở rộng thuộc tính để ChopSticks có thể đọc

        bool isControl = true;
        bool isDone = false;
        Vector3 ogPoint;
        Quaternion ogRot;

        private float currentCookProgress = 0f; // Độ chín hiện tại (0 -> 1)

        private void Awake()
        {
            OnSavePoint();
            // Khởi tạo alpha ban đầu (100% sống, 0% chín)
            SetAlPha(0);
        }

        public void OnSavePoint()
        {
            ogPoint = transform.position;
            ogRot = transform.rotation;
        }

        [Button]
        public void OnMix()
        {
            if (isControl && !isDone)
            {
                isControl = false;
                Vector3 targetPoint = center + new Vector3(
                    UnityEngine.Random.Range(-radius, radius),
                    UnityEngine.Random.Range(-0.1f, radius),
                    0
                );

                float randomRotateZ = UnityEngine.Random.Range(-180f, 180f);
                StartCoroutine(MixCoroutine(targetPoint, randomRotateZ));
            }
        }

        public void SetAlPha(float alpha)
        {
            if (rawSR == null) return;

            // Hàm này giữ nguyên logic của bạn (Giả sử bạn đã có extension method SetAlpha)
            if (alpha <= 1)
            {
                rawSR.SetAlpha(1 - alpha);
                doneSR.SetAlpha(alpha);
            }
            else if (alpha < 2)
            {
                doneSR.SetAlpha(2 - alpha);
            }
        }

        public void SetRadius(float radius)
        {
            this.radius = radius;
        }

        public void SetControl(bool isOn)
        {
            isControl = isOn;
            col.enabled = isOn;
        }

        [Button]
        public void OnDone()
        {
            if (isDone) return; // Tránh gọi nhiều lần
            isDone = true;

            StopAllCoroutines();
            StartCoroutine(MixCoroutine(ogPoint, ogRot.eulerAngles.z, true)); // Nhảy về vị trí cũ

            // Bắn sự kiện ra ngoài cho ChopSticks biết
            OnItemDoneEvent?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(center, radius);
        }

        IEnumerator MixCoroutine(Vector3 target, float rotateZ, bool isFinalJump = false)
        {
            Vector3 startPos = TF.position;
            Quaternion startRot = TF.rotation;
            Quaternion targetRot = Quaternion.Euler(0, 0, rotateZ);

            // Tính toán độ chín mục tiêu cho lần nảy này
            float startAlpha = currentCookProgress;
            float targetAlpha = isFinalJump ? 1f : Mathf.Min(1f, currentCookProgress + cookStep);

            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;

                Vector3 pos = Vector3.Lerp(startPos, target, t);
                float jumpOffset = Mathf.Sin(t * Mathf.PI) * jumpower;
                pos.y += jumpOffset;

                TF.position = pos;
                TF.rotation = Quaternion.Lerp(startRot, targetRot, t);

                // Mượt mà đổi màu (chuyển alpha) trong lúc đang bay lơ lửng
                float currentStepAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                SetAlPha(currentStepAlpha);

                time += Time.deltaTime;
                yield return null;
            }

            TF.position = target;
            TF.rotation = targetRot;

            // Chốt lại alpha khi tiếp đất
            currentCookProgress = targetAlpha;
            SetAlPha(currentCookProgress);

            isControl = true;

            // Kiểm tra xem nấu xong chưa, nếu đạt 100% thì gọi OnDone
            if (currentCookProgress >= 1f && !isDone && !isFinalJump)
            {
                OnDone();
            }
        }
    }
}