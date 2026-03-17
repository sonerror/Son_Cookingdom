using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class RollController : MonoBehaviour
    {
        private float hitDelay = 0.15f;
        private float lastHitTime;

        private void OnTriggerStay2D(Collider2D collision)
        {
            MochiInTray mochi = collision.GetComponent<MochiInTray>();
            if (mochi == null) return;

            if (Time.time - lastHitTime < hitDelay) return;

            lastHitTime = Time.time;

            mochi.Hit();
        }
    }
}