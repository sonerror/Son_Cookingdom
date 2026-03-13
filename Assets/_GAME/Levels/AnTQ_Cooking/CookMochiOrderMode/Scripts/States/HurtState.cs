using UnityEngine;

namespace sonnv
{
    public class HurtState : IState<CharacterBase>
    {
        public void OnEnter(CharacterBase t)
        {
            t.OnHurtEnter();
        }

        public void OnExecute(CharacterBase t)
        {
            t.OnHurtExecute();
        }

        public void OnExit(CharacterBase t)
        {
            t.OnHurtExit();
        }
    }
}