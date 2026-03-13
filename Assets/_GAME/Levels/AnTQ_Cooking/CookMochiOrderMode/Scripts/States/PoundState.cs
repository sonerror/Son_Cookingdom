using UnityEngine;

namespace sonnv
{
    public class PoundState : IState<CharacterBase>
    {
        public void OnEnter(CharacterBase t)
        {
            t.OnPoundEnter();
        }

        public void OnExecute(CharacterBase t)
        {
            t.OnPoundExecute();
        }

        public void OnExit(CharacterBase t)
        {
            t.OnPoundExit();
        }
    }
}