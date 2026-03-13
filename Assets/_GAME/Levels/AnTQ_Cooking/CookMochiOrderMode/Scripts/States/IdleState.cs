using UnityEngine;

namespace sonnv
{
    public class IdleState : IState<CharacterBase>
    {
        public void OnEnter(CharacterBase t)
        {
            t.OnIdleEnter();
        }

        public void OnExecute(CharacterBase t)
        {
            t.OnIdleExecute();
        }

        public void OnExit(CharacterBase t)
        {
            t.OnIdleExit();
        }
    }
}