using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class GaintPointEffect : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txtPoint;
        public void DisplayPoint(float point)
        {
            txtPoint.DOKill();
            txtPoint.color = Color.green;
            Vector2 offset = new Vector2(Random.Range(-.5f, .5f), Random.Range(-0.5f, 0f));
            txtPoint.transform.position = (Vector2)transform.position + offset;

            txtPoint.text = "+"+ point;
            txtPoint.DOFade(0, 1.5f);

            float y = txtPoint.transform.position.y;
            txtPoint.transform.DOMoveY(y + 1f, 1f);
        }
    }
}

