using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class DonutInPanHolder : IngredientHolder
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public bool isChangeAnim;
        public void HideSprite()
        {
            spriteRenderer.SetAlpha(0);
        }
        private void Awake()
        {
            animator.enabled = false;
        }
        public void PlayAnim()
        {
            animator.enabled = true;
            animator.SetTrigger("Flip");
        }
    }

}
