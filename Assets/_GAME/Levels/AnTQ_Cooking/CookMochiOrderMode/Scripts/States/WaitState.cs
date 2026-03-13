using UnityEngine;

namespace sonnv
{
    public class WaitState : IState<CharacterBase>
    {
        public void OnEnter(CharacterBase t)
        {
            t.OnWaitEnter();
        }

        public void OnExecute(CharacterBase t)
        {
            t.OnWaitExecute();
        }

        public void OnExit(CharacterBase t)
        {
            t.OnWaitExit();
        }
    }
}