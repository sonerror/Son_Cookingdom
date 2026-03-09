using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class IngredientForwarderAnim : IngredientForwarder
    {
        [SerializeField] private Animation anim;

        public float GetTimeAnim(string name)
        {
            return anim[name].clip.length;
        }
        public Animation GetAnim()
        {
            return anim;
        }
        public void PalyAnim()
        {
            anim.Play();
        }

    }
}

