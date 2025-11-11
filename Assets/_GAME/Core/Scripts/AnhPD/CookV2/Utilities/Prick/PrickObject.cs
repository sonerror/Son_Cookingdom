using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnhPD.CookV2
{
    public class PrickObject : MonoBehaviour
    {
        [SerializeField] private RandomSprite obj, effect;
        [SerializeField] private float angleOffset = 15f;
        
        public bool IsDone {private set; get;}
        
        public void Appear()
        {
            transform.localEulerAngles = new Vector3(0, 0, Random.Range(-angleOffset, angleOffset));
            obj.Randomize();
        }
        public void OnHit()
        {
            transform.Appear();
            obj.HideSprite();
            effect.Randomize();
            IsDone = true;
        }

        [Button]
        public void Restart()
        {
            obj.HideSprite();
            effect.HideSprite();
            IsDone = false;
        }

    #if UNITY_EDITOR
        [Button]
        private void Generate()
        {
            obj.render = Instantiate(new GameObject("object"), transform).AddComponent<SpriteRenderer>();
            effect.render = Instantiate(new GameObject("effect"), transform).AddComponent<SpriteRenderer>();
        }

        [Button]
        private void Show()
        {
            obj.render.enabled = true;
            effect.render.enabled = true;
        }
    #endif
    }

    [System.Serializable]
    public class RandomSprite
    {
        public SpriteRenderer render;
        [SerializeField] private Sprite[] sprites;

        public void Randomize()
        {
            if(render)
                render.enabled = true;
            
            if(sprites.Length > 0) 
                render.sprite = sprites[Random.Range(0, sprites.Length)];
        }
        public void HideSprite()
        {
            if(render)
                render.enabled = false;
        }
    }
}
