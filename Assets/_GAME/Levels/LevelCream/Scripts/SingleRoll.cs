using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace sonnv
{
    public class SingleRoll : MonoBehaviour
    {
        [SerializeField] private Transform mask;
        [SerializeField] private SpriteRenderer roll;
        [SerializeField] private Sprite[] rollSprites;
        [SerializeField] private Vector3 maskStarPos;
        [SerializeField] private float maskMoveDistance = 2f;

        public Transform startPos, endPos;

        private void GetMaskPos()
        {
            maskStarPos = mask.localPosition;
        }

        public void Init()
        {
            mask.localPosition = maskStarPos;
            roll.sprite = rollSprites[0];
        }

        public void StartRolling()
        {
            _timer = 0f;
            roll.enabled = true;
        }

        public void CompleteRolling()
        {
            roll.enabled = false;
        }
        private float _timer;
        public float Rate => Mathf.Clamp01(_timer / 1f);
        public void OnRolling(float delta)
        {
            _timer += delta;
            mask.localPosition = maskStarPos + mask.up * maskMoveDistance * Rate;

            int index = Mathf.FloorToInt((rollSprites.Length - 1) * Rate);
            roll.sprite = rollSprites[index];
        }

        public Vector3 GetSpatulaPosition()
        {
            return Vector3.Lerp(startPos.position, endPos.position, Rate);
        }
    }
}
