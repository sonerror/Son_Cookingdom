using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.CookV2
{
    public class APDProgressionLevelBase : APDLevelBase
    {
        [SerializeField] private APDProgressionStage[] progressStages;

        [ShowInInspector, ReadOnly] private int _stageIndex;
        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < progressStages.Length; i++)
            {
                APDProgressionStage stage = progressStages[i];
                stage.OnHintChange += HandleHintChange;
                stage.OnCompleteStage += NextStage;
                stage.stageIndex = i;
                
                stage.Register();
            }
        }
        
        private void HandleHintChange(Sprite sprite)
        {
            _hint = sprite;
            Debug.Log(_hint.name);
        }

        private void NextStage(int stageIndex)
        {
            _stageIndex = stageIndex;
            _stageIndex++;
            if (_stageIndex >= progressStages.Length)
            {
                EndGame();
                return;
            }
            progressStages[_stageIndex].OnEnter();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (APDProgressionStage stage in progressStages)
            {
                stage.OnHintChange -= HandleHintChange;
                stage.OnCompleteStage -= NextStage;
                
                stage.Unregister();
            }
        }
        
        //-------------For Test----------------
        public void NextPhase()
        {
            progressStages[_stageIndex].ForceCompletePhase();
        }
        //-------------------------------------
    }
}
