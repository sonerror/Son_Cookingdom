using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using UnityEngine;
using UnityEngine.Events;

public class Candy : MonoBehaviour
{
  public float timeDelay = 0.5f;
  [SerializeField] protected Transform target, center;
  [SerializeField] private BeforeCompleteAction beforeCompleteAction;

  public UnityEvent clickEvent;


  public void PlayAction()
  {
    StartCoroutine(PlayActionCoroutine());
  }

  private IEnumerator PlayActionCoroutine()
  {
    yield return new WaitForSeconds(timeDelay);
    clickEvent?.Invoke();

  }
}
