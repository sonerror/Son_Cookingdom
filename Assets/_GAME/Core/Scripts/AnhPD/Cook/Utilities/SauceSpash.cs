using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class SauceSpash : MonoBehaviour
  {
    [SerializeField] private float deltaX, deltaY;
    [SerializeField] private Transform sauce;
    [SerializeField] private APDCookingToolBase sauce_root;
    [SerializeField] private SpriteRenderer sauceRenderer;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float gap = .2f;
    [SerializeField] private AudioClip sfxSauce;

    public Vector3 SaucePos => sauce.position;
    public UnityEvent startEvent, endEvent;
    public void Spash()
    {
      gameObject.SetActive(true);
      sauce_root.EnableSprite(false);
      sauce_root.EnableCollider(false);
      startEvent?.Invoke();
      float x = sauce.position.x + deltaX;
      // AudioManager.PlaySFX(sfxSauce);
      sauce.DOMoveX(x, duration).OnComplete(() =>
      {
        sauce.gameObject.SetActive(false);
        endEvent?.Invoke();
        sauceRenderer.maskInteraction = SpriteMaskInteraction.None;

        sauce_root.Tf.DOComplete(true);
        sauce_root.EnableSprite(true);
        sauce_root.EnableCollider(true);
      });

      float y = sauce.position.y;
      Sequence seq = DOTween.Sequence();
      int count = Mathf.FloorToInt(duration / gap);
      for (int i = 0; i < count; i++)
      {
        seq.AppendCallback(() =>
        {
          sauce.DOMoveY(y + deltaY / 2, gap / 2).OnComplete(() =>
                  {
              sauce.DOMoveY(y - deltaY / 2, gap / 2);
            });
        });
        seq.AppendInterval(gap);
      }
      seq.Play();
    }
  }
}

