using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class SonSinkWaterCleaning : SonMonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PipeWaterCleaning drainPipe;
        [SerializeField] private Animator water;
        [SerializeField] private Collider2D btnCollider;
        [SerializeField] private SpriteRenderer btnOn;
        [SerializeField] private SpriteRenderer btnOff;
        [Header("Events")]
        [SerializeField] private UnityEvent onFillWater;
        [SerializeField] private UnityEvent onStopFillWater;
        [SerializeField] private UnityEvent onDrainWater;
        [Header("Sound")]
        [SerializeField] private AudioData btnSound;
        [SerializeField] private AudioData waterOutSound;
        [SerializeField] private AudioSource waterFillSound;

        private const string FILL_WATER = "fill";
        private const string STOP_FILL = "full";
        private const string DRAIN_WATER = "drain";

        [ShowInInspector][ReadOnly] private string _currentState;
        private bool _isFillWater;
        private bool _canInteract;
        private bool _addAdditionalInteract;

        public bool HasWater => _currentState is FILL_WATER or STOP_FILL;
        public bool IsFillWater => _isFillWater;
        public bool IsInHold => drainPipe.IsInHole;
        public PipeWaterCleaning DrainPipe => drainPipe;
        public readonly HashSet<WaterCleaningSnapObject> needCleanSnapObjects = new();
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

        private void SetPipeInteract(bool canInteract)
        {
            drainPipe.SetInteract(canInteract);
        }

        private bool ChangeWaterAnimation(string state)
        {
            if (_currentState == state) return false;
            water.ResetTrigger(_currentState);
            water.SetTrigger(state);
            _currentState = state;

            #region Special Case for Audio

            if (_currentState == FILL_WATER)
            {
                waterFillSound.Play();
            }
            else
            {
                waterFillSound.Stop();
            }

            #endregion

            return true;
        }

        private void FillWater()
        {
            if (!drainPipe.IsInHole) return;
            if (!ChangeWaterAnimation(FILL_WATER)) return;
            SoundManager.PlaySFX(btnSound.clip, btnSound.volume);
            _isFillWater = true;
            btnOn.enabled = true;
            btnOff.enabled = false;
            SetPipeInteract(false);
            onFillWater.Invoke();
            CleaningObject();
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
            SetPipeInteract(true);
            onStopFillWater?.Invoke();
        }

        private void CleaningObject()
        {
            var objectsToRemove = new List<WaterCleaningSnapObject>();

            foreach (var snapObject in needCleanSnapObjects)
            {
                if (snapObject.TryEnableByFillWater())
                {
                    objectsToRemove.Add(snapObject);
                }
            }

            foreach (var snapObject in objectsToRemove)
            {
                needCleanSnapObjects.Remove(snapObject);
            }
        }

        private void OnMouseUpAsButton()
        {
            if (!_canInteract) return;

            if (_isFillWater)
            {
                StopFillWater();
            }
            else
            {
                FillWater();
            }
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
                onDrainWater?.Invoke();
            }
        }
    }

}

