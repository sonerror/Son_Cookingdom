using UnityEngine;

namespace sonnv
{
    public class KneadState : IState<CharacterBase>
    {
        public void OnEnter(CharacterBase t)
        {
            t.OnKneadEnter();
        }

        public void OnExecute(CharacterBase t)
        {
            t.OnKneadExecute();
        }

        public void OnExit(CharacterBase t)
        {
            t.OnKneadExit();
        }
    }
}