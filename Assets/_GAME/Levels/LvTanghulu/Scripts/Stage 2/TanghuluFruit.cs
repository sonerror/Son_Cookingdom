using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnhPD.Tanghulu
{
    public class TanghuluFruit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer coating;
        [SerializeField] private Sprite[] coatingSprites;
        [SerializeField] private Transform[] decors;

        [SerializeField] private float height = .5f;

        private Vector3 _pos;
        
        public Vector3 Top => transform.position + transform.up * height;
        public Vector3 LocalTop => _pos + transform.up * height;
        public float Height => height;

        public void SetPos(Vector3 pos)
        {
            transform.Appear();
            ResetAll();
            _pos = pos; 
            transform.position = pos;
        }

        public void ShowCoat(int coatIndex)
        {
            coating.enabled = true;
            coating.sprite = coatingSprites[coatIndex];
        }

        public void ShowDecor(int index, bool isFall = false)
        {
            if(isFall) decors[index].FallAppear();
            else decors[index].gameObject.SetActive(true);
            int rand = Random.Range(0, 2);
            decors[index].eulerAngles = new Vector3(0, rand < 1 ? 0 : 180f, Random.Range(-3f, 3f));
        }

        private void HideDecor()
        {
            foreach (Transform decor in decors) decor.gameObject.SetActive(false);
        }

        public void ResetAll()
        {
            coating.enabled = false;
            HideDecor();
        }
    }
}
