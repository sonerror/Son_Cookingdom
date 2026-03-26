using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace sonnv
{
    public class ChopSticks : SonMonoBehaviour
    {
        [SerializeField] private FlourMoveToCream moveObject;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private UnityEvent onDone;

        [SerializeField] Collider2D col;
        [SerializeField] private int totalItemsInPot = 3;
        private Dictionary<Collider2D, ItemMixJump> items = new Dictionary<Collider2D, ItemMixJump>();
        IItemCooking itemCooking;

        private float lastSoundTime = -1f;
        [SerializeField] private float soundCooldown = 1f;

        private bool isGameWon = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IItemCooking item))
            {
                this.itemCooking = item;
            }
            if (!items.ContainsKey(collision))
            {
                ItemMixJump mixItem = collision.GetComponent<ItemMixJump>();
                if (mixItem != null)
                {
                    items.Add(collision, mixItem);
                    mixItem.OnItemDoneEvent += CheckWinCondition;
                }
            }
            if (items.ContainsKey(collision) && items[collision] != null)
            {
                items[collision].OnMix();

                if (!isGameWon && Time.time >= lastSoundTime + soundCooldown)
                {
                    SoundManager.PlaySFXOneShot(jumpSound);
                    lastSoundTime = Time.time;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<IItemCooking>() != null)
            {
                this.itemCooking = null;
            }
        }

        private void CheckWinCondition()
        {
            if (isGameWon) return;

            if (items.Count < totalItemsInPot) return;

            bool isAllCooked = true;
            foreach (var kvp in items)
            {
                if (kvp.Value != null && !kvp.Value.IsDone)
                {
                    isAllCooked = false;
                    break;
                }
            }

            if (isAllCooked)
            {
                onDone?.Invoke();
                StartCoroutine(IE_DelayMove());
                isGameWon = true;
            }
        }
        IEnumerator IE_DelayMove()
        {
            yield return new WaitForSeconds(0.5f);
            moveObject.OnMove();
        }

    }
}