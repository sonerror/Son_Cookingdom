using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.KingCrab
{
    public class CrabBowl : MonoBehaviour
    {
        [SerializeField] Transform sauce;
        [SerializeField] Transform meat, meat2;
        public void OnSauceIn()
        {
            sauce.gameObject.SetActive(true);
            sauce.DOPunchScale(Vector3.one * .1f, .3f);
        }
        public void OnMeatIn()
        {
            meat.gameObject.SetActive(true);
            meat.DOPunchScale(Vector3.one * .1f, .3f);

            sauce.gameObject.SetActive(false);
            LevelKingCrab.Instance.OnPutMeatInBow();
        }
        public void OnMeatIn2()
        {
            meat2.gameObject.SetActive(true);
            meat2.DOPunchScale(Vector3.one * .1f, .3f);

            meat.gameObject.SetActive(false);
            LevelKingCrab.Instance.OnPutMeatInBow2();
        }
    }
}

