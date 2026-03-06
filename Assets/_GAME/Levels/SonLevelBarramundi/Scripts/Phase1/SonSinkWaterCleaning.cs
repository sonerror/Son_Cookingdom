using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace sonnv
{
    public class SonSinkWaterCleaning : SonMonoBehaviour, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] private PipeWaterCleaning drainPipe;
        [SerializeField] private Animator water;
        [SerializeField] private Collider2D btnCollider;
        [SerializeField] private SpriteRenderer btnOn;
        [SerializeField] private SpriteRenderer btnOff;

        [Header("Events")]
        [SerializeField] private UnityEvent onFillWater;
        public UnityEvent OnFillWater => onFillWater;
        [SerializeField] private UnityEvent onStopFillWater;
        [SerializeField] private UnityEvent onDrainWater;

        [Header("Sound")]
        [SerializeField] private AudioData btnSound;
        [SerializeField] private AudioData waterOutSound;
        [SerializeField] private AudioSource waterFillSound;

        private const string FILL_WATER = "fill";
        private const string STOP_FILL = "full";
        private const string DRAIN_WATER = "drain";

        [SerializeField] private string _currentState;

        private bool _isFillWater;
        private bool _canInteract;
        private bool _ignoreNextClick;

        public bool HasWater => _currentState == FILL_WATER || _currentState == STOP_FILL;
        public bool IsFillWater => _isFillWater;
        public bool IsInHold => drainPipe.IsInHole;
        public PipeWaterCleaning DrainPipe => drainPipe;

        public List<WaterCleaningSnapObject> needCleanSnapObjects = new List<WaterCleaningSnapObject>();
        private List<WaterCleaningSnapObject> _objectsToRemove = new List<WaterCleaningSnapObject>();

        public UnityEvent OnStopFillWater => onStopFillWater;

        private void Awake()
        {
            SetCanInteract(true);
            drainPipe.OnStartDrag.AddListener(DrainWater);
        }

        public void SetCanInteract(bool canInteract)
        {
            _canInteract = canInteract;
            btnCollider.enabled = canInteract;
        }

        public void IgnoreNextClick()
        {
            _ignoreNextClick = true;
        }

        private void SetPipeInteract(bool canInteract)
        {
            drainPipe.SetInteract(canInteract);
        }

        private bool ChangeWaterAnimation(string state)
        {
            if (_currentState == state) return false;

            if (!string.IsNullOrEmpty(_currentState))
                water.ResetTrigger(_currentState);

            water.SetTrigger(state);

            _currentState = state;

            if (_currentState == FILL_WATER)
                waterFillSound.Play();
            else
                waterFillSound.Stop();

            return true;
        }

        private void FillWater()
        {
            if (!drainPipe.IsInHole) return;
            SetCanInteract(false);
            if (!ChangeWaterAnimation(FILL_WATER)) return;

            SoundManager.PlaySFX(btnSound.clip, btnSound.volume);

            _isFillWater = true;

            btnOn.enabled = true;
            btnOff.enabled = false;

            SetPipeInteract(false);

            if (onFillWater != null)
                onFillWater.Invoke();

            CleaningObject();
            if (autoStopCoroutine != null)
                StopCoroutine(autoStopCoroutine);

            autoStopCoroutine = StartCoroutine(AutoStopWater());
        }

        public void SetIsInHole(bool value)
        {
            drainPipe.SetIsInHole(value);
        }

        private void StopFillWater()
        {
            if (!ChangeWaterAnimation(STOP_FILL)) return;

            SoundManager.PlaySFX(btnSound.clip, btnSound.volume);

            _isFillWater = false;

            btnOn.enabled = false;
            btnOff.enabled = true;

            // SetPipeInteract(true);

            if (onStopFillWater != null)
                onStopFillWater.Invoke();
        }
        public void OnSetPipeInteract(bool value)
        {
            SetPipeInteract(value);
        }
        private void CleaningObject()
        {
            _objectsToRemove.Clear();

            for (int i = 0; i < needCleanSnapObjects.Count; i++)
            {
                var snapObject = needCleanSnapObjects[i];

                if (snapObject.TryEnableByFillWater())
                {
                    _objectsToRemove.Add(snapObject);
                }
            }

            for (int i = 0; i < _objectsToRemove.Count; i++)
            {
                needCleanSnapObjects.Remove(_objectsToRemove[i]);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_ignoreNextClick)
            {
                _ignoreNextClick = false;
                return;
            }

            if (!_canInteract) return;

            if (_isFillWater)
                StopFillWater();
            else
                FillWater();
        }

        public void OnFillWaterDone()
        {
            StopFillWater();
        }

        private void DrainWater()
        {
            if (drainPipe.IsInHole) return;

            if (HasWater && ChangeWaterAnimation(DRAIN_WATER))
            {
                SoundManager.PlaySFX(waterOutSound.clip, waterOutSound.volume);

                if (onDrainWater != null)
                    onDrainWater.Invoke();
            }
        }
        private Coroutine autoStopCoroutine;
        private IEnumerator AutoStopWater()
        {
            yield return new WaitForSeconds(3f);

            if (_isFillWater)
            {
                StopFillWater();
            }
        }
    }
}