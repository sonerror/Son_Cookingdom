using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class Sugarcane : MonoBehaviour
  {
    [SerializeField] private GameObject[] masks;
    [SerializeField] private GameObject sugarcane, caneStalk;
    [SerializeField] private AudioClip sfxCut;
    private int _index;
    public bool isComplete { get; private set; }

    public Vector3 GetMaskPosition()
    {
      int maskIndex = _index;
      if (maskIndex >= masks.Length) maskIndex = masks.Length - 1;
      return masks[maskIndex].transform.position;
    }
    [Button]
    public void OnCut()
    {
      // AudioManager.PlaySFX(sfxCut);
      transform.DOPunchRotation(new Vector3(0, 0, 5f), .3f);
      if (_index >= masks.Length)
      {
        StartCoroutine(CompleteSFX());
        caneStalk.SetActive(true);
        sugarcane.SetActive(false);
        isComplete = true;
        return;
      }
      masks[_index].SetActive(true);
      _index++;
    }

    private IEnumerator CompleteSFX()
    {
      yield return new WaitForSeconds(.05f);
      // AudioManager.PlaySFxRandomPitch(sfxCut);
      yield return new WaitForSeconds(.05f);
      // AudioManager.PlaySFxRandomPitch(sfxCut);
      yield return new WaitForSeconds(.05f);
      // AudioManager.PlaySFxRandomPitch(sfxCut);
    }
  }

}