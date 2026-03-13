using UnityEngine;

namespace sonnv
{
    public class SorryState : IState<CharacterBase>
    {
        public void OnEnter(CharacterBase t)
        {
            t.OnSorryEnter();
        }

        public void OnExecute(CharacterBase t)
        {
            t.OnSorryExecute();
        }

        public void OnExit(CharacterBase t)
        {
            t.OnSorryExit();
        }
    }
}