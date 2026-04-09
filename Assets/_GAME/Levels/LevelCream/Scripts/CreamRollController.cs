using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class CreamRollController : MonoBehaviour
    {
        [SerializeField] private Vector3 startPos;
        public Vector3 StartPos => startPos;
        [SerializeField] private Vector3 endPos;
        public Vector3 EndPos => endPos;
        [SerializeField] private SpriteRenderer[] renderers;
        [SerializeField] private SpriteRenderer[] renderersDone;
        [SerializeField] private SingleRoll[] rolls;
        [SerializeField] private Roller roller;
        public UnityEvent onAllRollsCompleted;
        private int _index = 0;

        // Thêm biến quy định số lượng cuộn cùng lúc (có thể chỉnh trong Inspector nếu muốn)
        [SerializeField] private int _rollStep = 2;

        public void Init(Color color)
        {
            foreach (var r in renderers) r.color = color;
            foreach (SingleRoll roll in rolls) roll.Init();
            gameObject.SetActive(true);
            _index = 0;
        }

        // Trong CreamRollController.cs
        public void StartRoll()
        {
            roller.gameObject.SetActive(true);

            // Mặc định lấy vị trí và góc quay của cuộn hiện tại
            Vector3 targetPos = rolls[_index].startPos.position;
            Vector3 targetRot = rolls[_index].startPos.eulerAngles;

            // Nếu cuộn 2 cái cùng lúc, tính trung điểm giữa startPos của cuộn 1 và cuộn 2
            if (_rollStep > 1 && _index + 1 < rolls.Length)
            {
                Vector3 pos1 = rolls[_index].startPos.position;
                Vector3 pos2 = rolls[_index + 1].startPos.position;
                targetPos = (pos1 + pos2) / 2f; // Đây là vị trí chính giữa 2 cuộn
            }

            // Gọi hàm di chuyển với tọa độ đã tính
            roller.MoveToNextRoll(targetPos, targetRot);
            startPos = targetPos;
            if (_rollStep > 1 && _index + 1 < rolls.Length)
            {
                Vector3 pos1 = rolls[_index].endPos.position;
                Vector3 pos2 = rolls[_index + 1].endPos.position;
                endPos = (pos1 + pos2) / 2f;
            }
        }

        private void NextRoll()
        {
            for (int i = 0; i < _rollStep; i++)
            {
                if (_index + i < renderersDone.Length)
                    renderersDone[_index + i].gameObject.SetActive(true);
            }

            _index += _rollStep;

            if (_index < rolls.Length)
            {
                // Khi chuyển sang cặp tiếp theo, cũng tính lại trung điểm
                Vector3 nextTargetPos = rolls[_index].startPos.position;
                Vector3 nextTargetRot = rolls[_index].startPos.eulerAngles;

                if (_rollStep > 1 && _index + 1 < rolls.Length)
                {
                    nextTargetPos = (rolls[_index].startPos.position + rolls[_index + 1].startPos.position) / 2f;
                }

                roller.MoveToNextRoll(nextTargetPos, nextTargetRot);
                startPos = nextTargetPos;
                endPos = (rolls[_index].endPos.position + rolls[_index + 1].endPos.position) / 2f; ;
            }
            else
            {
                roller.gameObject.SetActive(false);
                gameObject.SetActive(false);
                _index = 0;
                onAllRollsCompleted?.Invoke();
                Debug.Log("Win Roll");
            }
        }
        public void ShowCurrentRoll()
        {
            // Kích hoạt rolling cho cả 2 cuộn
            for (int i = 0; i < _rollStep; i++)
            {
                if (_index + i < rolls.Length)
                {
                    rolls[_index + i].StartRolling();
                }
            }
        }

        public void OnRolling(float delta)
        {
            // Truyền giá trị kéo (delta) cho cả 2 cuộn
            for (int i = 0; i < _rollStep; i++)
            {
                if (_index + i < rolls.Length)
                {
                    rolls[_index + i].OnRolling(delta);
                }
            }

            // Cập nhật vị trí dụng cụ cuộn (Roller)
            // Lấy vị trí trung bình giữa 2 cuộn để Roller nằm ở giữa trông tự nhiên hơn
            Vector3 targetPos = rolls[_index].GetSpatulaPosition();
            if (_index + 1 < rolls.Length)
            {
                Vector3 pos2 = rolls[_index + 1].GetSpatulaPosition();
                targetPos = (targetPos + pos2) / 2f; // Chia đôi lấy trung điểm
            }
            roller.SetPosition(targetPos);

            // Kiểm tra tiến trình dựa vào cuộn đầu tiên của cặp
            if (rolls[_index].Rate >= 1f)
            {
                // Hoàn thành cả 2 cuộn
                for (int i = 0; i < _rollStep; i++)
                {
                    if (_index + i < rolls.Length)
                    {
                        rolls[_index + i].CompleteRolling();
                    }
                }
                NextRoll();
            }
        }
    }
}