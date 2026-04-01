using System.Collections.Generic;
using DG.Tweening;

using sonnv;
using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class NextStepLevel : SonLevelBase
    {
        #region Base

        [Header("Step Tracking")]

        [SerializeField] private int currentStep;
        private readonly HashSet<int> _doneSteps = new HashSet<int>();

        // NOTE: CurrentStep is already declare in LevelBase (For track Firebase), but it's not related to this currentStep (For track StepByStep)
        // Use CurStep for keep track StepByStep, not CurrentStep
        public int CurStep => currentStep;


        [SerializeField]
        protected EmojiControl emoji;

        [SerializeField] private UnityEvent onEndGame;
        [SerializeField] private float delayTimeEndGame = 5f;

        #endregion

        #region Control Step

        private readonly Dictionary<int, System.Action> _stepActions = new Dictionary<int, System.Action>();
        protected int StepActionCount => _stepActions.Count;

        protected override void Awake()
        {
            base.Awake();
            InitStepActions();
        }


        protected virtual void InitStepActions() { }

        protected void AddStepAction(int step, System.Action action)
        {
            _stepActions.Add(step, action);
        }

        protected void RemoveStepAction(int step)
        {
            _stepActions.Remove(step);
        }

        public virtual void OnWrongCurrentStep()
        {
            emoji.ShowNegative();
        }

        private bool IsCurrentStepDone()
        {
            return _doneSteps.Contains(currentStep);
        }

        protected virtual void DoneStep(bool showEmoji = true, bool playSound = false)
        {
            _doneSteps.Add(currentStep);
            if (showEmoji)
            {
                if (playSound)
                {
                    //     emoji.ShowPositiveWithSound();
                }
                else
                {
                    //   emoji.ShowPositive();
                }
            }
            // #if UNITY_EDITOR
            //             Debug.Log("Done Step: " + currentStep);
            // #endif
        }

        // BE CAREFUL WHEN USING THIS METHOD, IT WILL SKIP ALL BEFORE STEPS
        protected void ForceChangeCurStep(int step)
        {
            currentStep = step;
        }

        public void TryNextStep()
        {
            Debug.Log($"Trying to go to next step: {currentStep}");
            if (!IsCurrentStepDone()) return;
            OnNextStep();
        }

        protected virtual void OnNextStep()
        {
            currentStep++;
            SetStep(currentStep);
            if (_stepActions.TryGetValue(currentStep, out var action))
            {
                action.Invoke();
            }
        }

        protected virtual void OnEndGame()
        {
            isEndingGame = true;
            onEndGame?.Invoke();
            // this.WaitToDo(EndGame, delayTimeEndGame);
        }

        #endregion

    }
}
