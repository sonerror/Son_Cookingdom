using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class DirtyClean : MonoBehaviour
  {
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float duration = 3f;
    [SerializeField] protected AudioClip sfx;
    [SerializeField] protected DirtyManager manager;
    [SerializeField] protected float alpha = .4f;

    protected bool isReady;
    protected float timer, cooldown = .2f;
    [Button]
    private void FindSpriteRenderer()
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void Appear()
    {
      transform.FallAppear();
      timer = 0f;
      isReady = true;
    }
    public virtual void OnCleaning()
    {
      if (!isReady) return;
      timer += Time.deltaTime;
      float rate = Mathf.Clamp01(timer / duration);
      spriteRenderer.SetAlpha(alpha + (1f - rate) * (1 - alpha));
      if (timer >= duration)
      {
        gameObject.SetActive(false);
        manager.OnDirtyClean();
        isReady = false;
      }

      if (cooldown <= 0f)
      {
        // AudioManager.PlaySFX(sfx, .1f);
        cooldown = .2f;
      }
    }

    private void Update()
    {
      if (cooldown > 0f)
      {
        cooldown -= Time.deltaTime;
      }
    }

    public void Init(DirtyManager manager)
    {
      this.manager = manager;
    }
  }
}

