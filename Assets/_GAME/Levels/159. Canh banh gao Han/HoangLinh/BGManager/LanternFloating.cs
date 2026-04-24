using UnityEngine;
using System.Collections.Generic;

namespace HoangLinh.Cooking.Level159
{
    public class LanternFloating : MonoBehaviour
    {
        [Header("Move Settings")]
        [SerializeField] float moveRange = 0.5f;
        [SerializeField] float minSpeed = 1f;
        [SerializeField] float maxSpeed = 2f;

        private class LanternData
        {
            public Transform tf;
            public Vector3 initialPos;
            public float speed;
            public float randomOffset;
        }

        private List<LanternData> lanterns = new List<LanternData>();

        void Start()
        {
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeSelf) continue;

                LanternData data = new LanternData();
                data.tf = child;
                data.initialPos = child.localPosition;

                data.speed = Random.Range(minSpeed, maxSpeed);
                data.randomOffset = Random.Range(0f, 10f);

                lanterns.Add(data);
            }
        }

        void Update()
        {
            foreach (var lantern in lanterns)
            {
                float yOffset = Mathf.Sin(Time.time * lantern.speed + lantern.randomOffset) * moveRange;

                lantern.tf.localPosition = new Vector3(
                    lantern.initialPos.x,
                    lantern.initialPos.y + yOffset,
                    lantern.initialPos.z
                );
            }
        }
    }
}