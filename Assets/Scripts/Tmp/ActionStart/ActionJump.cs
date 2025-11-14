using DG.Tweening;
using Link;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Team1
{
  public class ActionJump : ActionBase
  {
    [Header("Configs")]
    [SerializeField] float duration;
    [SerializeField, BoxGroup("Transforms")] Transform start;
    [SerializeField, BoxGroup("Transforms")] Transform transition;
    [SerializeField, BoxGroup("Transforms")] Transform final;
    [SerializeField, BoxGroup("After action")] AudioClip onDoneSFX;
    [SerializeField, BoxGroup("After action")] ParticleSystem onDoneVFX;
    [SerializeField, BoxGroup("After action")] float vfxLength;

    public Transform TF { get; private set; }
    private void Awake()
    {
      TF = transform;
    }

    [Button]
    public override void Active()
    {
      // Init setup
      gameObject.SetActive(startActive);
      // Start action
      StartCoroutine(IECurveMovement());
    }

    protected override void OnDone()
    {

      base.OnDone();
    }

    private IEnumerator IECurveMovement()
    {
      yield return Cache.GetWFS(delay);
      gameObject.SetActive(true);
      TF.position = start.position;
      PlaySFX(clip);
      TF.DOScale(final.localScale, duration);
      TF.DORotateQuaternion(final.rotation, duration);
      float t = 0f;
      while (t < 1f)
      {
        // ease t
        t += Time.deltaTime / duration;
        float easeT = Mathf.SmoothStep(0f, 1f, t);
        Vector3 pos = Lerp3(start.position, transition.position, final.position, easeT);
        TF.position = new Vector3(pos.x, pos.y, 0);
        yield return null;
      }
      TF.position = new Vector3(final.position.x, final.position.y, TF.position.z);
      if (onDoneVFX != null)
      {
        onDoneVFX.transform.position = final.position;
        onDoneVFX.Play();
        PlaySFX(onDoneSFX);
      }
      yield return Cache.GetWFS(vfxLength);
      OnDone();
    }

    private void PlaySFX(AudioClip clip)
    {
      if (clip != null)
      {
        // SoundControl.Ins.PlayFX(clip);
      }
    }

    private void OnDrawGizmos()
    {
      if (start == null || transition == null || final == null)
        return;

      Gizmos.color = Color.red;

      Vector3 prevPoint = start.position;
      int segments = 30;

      for (int i = 1; i <= segments; i++)
      {
        float t = i / (float)segments;
        Vector3 pos = Lerp3(start.position, transition.position, final.position, t);
        Gizmos.DrawLine(prevPoint, pos);
        prevPoint = pos;
      }

      Gizmos.color = Color.blue;
      Gizmos.DrawSphere(start.position, 0.1f);
      Gizmos.DrawSphere(transition.position, 0.1f);
      Gizmos.DrawSphere(final.position, 0.1f);
    }

    Vector3 Lerp3(Vector3 a, Vector3 b, Vector3 c, float t)
    {
      Vector3 ab = Vector3.Lerp(a, b, t);
      Vector3 bc = Vector3.Lerp(b, c, t);
      return Vector3.Lerp(ab, bc, t);
    }
  }
}