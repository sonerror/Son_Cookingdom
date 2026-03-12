using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;
namespace sonnv
{
    public class BowlMixer : MonoBehaviour
    {

        [SerializeField] private SpoonRotateDrag spoonController;

        [SerializeField] private SonSnapObject spoonInBox;

        [SerializeField] private List<EllipseOrbit> ellipseOrbits;
        [SerializeField] private List<MeatPieceOnBowl> meatPieces;

        [SerializeField] private float meatOrbitSpeed = 0.5f;   // tốc độ quay nhẹ
        [SerializeField] private float mixAmountRequired = 200f;

        [SerializeField] protected UnityEvent onDone;
        public UnityEvent OnDone => onDone;
        private bool isStirring;
        private float mixAmount;
        [SerializeField] private bool isDone;
        public bool IsDone => isDone;

        void Start()
        {
            spoonController.OnPositionChanged += OnStartStir;
            spoonController.OnPositionUnChanged += OnStopStir;
        }
        [SerializeField] private float stirThreshold = 0.002f;

        [SerializeField] private SpriteRenderer rawSprRdr, mixedSprRdr;
        void Update()
        {
            if (!isStirring || isDone) return;

            float angleDelta = meatOrbitSpeed * Time.deltaTime;

            for (int i = 0; i < ellipseOrbits.Count; i++)
            {
                ellipseOrbits[i].AddAngle(angleDelta);
            }

            // tiến trình trộn
            mixAmount += Time.deltaTime * 40f;

            float progress = Mathf.Clamp01(mixAmount / mixAmountRequired);
            rawSprRdr.SetAlpha(1 - progress);
            mixedSprRdr.SetAlpha(1 - progress);
            for (int i = 0; i < meatPieces.Count; i++)
                meatPieces[i].ChangeAlphaMixed(progress);

            if (progress >= 1f)
                FinishMix();
        }

        void OnStartStir(float movement)
        {
            isStirring = movement > stirThreshold;

        }

        void OnStopStir()
        {
            isStirring = false;
        }
        [SerializeField] private GameObject objMeatMix;
        [SerializeField] private GameObject objMeatDone;

        void FinishMix()
        {
            isDone = true;

            spoonController.OnPositionChanged -= OnStartStir;
            spoonController.OnPositionUnChanged -= OnStopStir;
            spoonController.SetIsDone();
            spoonController.gameObject.SetActive(false);
            spoonInBox.MoveBack();
            SonUtilities.DelayedCallScaled(0.2f, () =>
               {
                   objMeatDone.SetActive(true);
                   onDone?.Invoke();
                   //    StartCoroutine(IEShowAnimDropIn(meatPieces, () =>
                   //    {
                   //    }));
               });
            Debug.Log("Mix Done");
        }
        [SerializeField] private List<GameObject> lstActionInDone;

        private IEnumerator IEShowAnimDropIn(List<MeatPieceOnBowl> lstActionIn, System.Action onDone = null)
        {
            int min = Mathf.Min(lstActionInDone.Count, lstActionIn.Count);
            for (int i = 0; i < min; i++)
            {
                lstActionIn[i].gameObject.SetActive(false);
                lstActionInDone[i].SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
            SonUtilities.DelayedCallScaled(0.4f, () =>
            {
                onDone?.Invoke();
            });
        }
    }
}