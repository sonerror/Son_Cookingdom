using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class TapButton : MonoBehaviour
  {
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private AudioClip sfxClick;

    public bool IsReady;
    public bool isAlwaysReady;
    public UnityEvent clickEvent;

    private void OnMouseDown()
    {
      if (!IsReady) return;

      if (!isAlwaysReady)
        IsReady = false;
      // AudioManager.PlaySFX(sfxClick);
      clickEvent?.Invoke();
    }
    public void EnableCollider(bool isEnable = true)
    {
      coll2D.enabled = isEnable;
    }

    public void OnReady()
    {
      IsReady = true;
    }

  }
}

