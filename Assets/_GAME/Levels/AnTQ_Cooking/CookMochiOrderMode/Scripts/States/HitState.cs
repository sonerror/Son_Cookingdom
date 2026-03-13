using UnityEngine;

namespace sonnv
{
    public class HitState : IState<CharacterBase>
    {
        public void OnEnter(CharacterBase t)
        {
            t.OnHitEnter();
        }

        public void OnExecute(CharacterBase t)
        {
            t.OnHitExecute();
        }

        public void OnExit(CharacterBase t)
        {
            t.OnHitExit();
        }
    }
}