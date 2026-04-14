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
        [SerializeField] private int _rollStep = 2;

        public void Init(Color color)
        {
            foreach (var r in renderers) r.color = color;
            foreach (SingleRoll roll in rolls) roll.Init();
            gameObject.SetActive(true);
            _index = 0;
        }
        public void StartRoll()
        {
            roller.gameObject.SetActive(true);
            Vector3 targetPos = rolls[_index].startPos.position;
            Vector3 targetRot = rolls[_index].startPos.eulerAngles;
            if (_rollStep > 1 && _index + 1 < rolls.Length)
            {
                Vector3 pos1 = rolls[_index].startPos.position;
                Vector3 pos2 = rolls[_index + 1].startPos.position;
                targetPos = (pos1 + pos2) / 2f;
            }
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
            for (int i = 0; i < _rollStep; i++)
            {
                if (_index + i < rolls.Length)
                {
                    rolls[_index + i].OnRolling(delta);
                }
            }

            Vector3 targetPos = rolls[_index].GetSpatulaPosition();
            if (_index + 1 < rolls.Length)
            {
                Vector3 pos2 = rolls[_index + 1].GetSpatulaPosition();
                targetPos = (targetPos + pos2) / 2f;
            }
            roller.SetPosition(targetPos);

            if (rolls[_index].Rate >= 1f)
            {
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