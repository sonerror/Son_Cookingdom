using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class OrbitChangeByMoveAroundObject : MonoBehaviour
    {
        [SerializeField] private List<EllipseOrbit> ellipseOrbits;
        [SerializeField] private SpoonRotateDrag spoonController;
        [SerializeField] private float speedMultiplier = 1f;

        public List<EllipseOrbit> EllipseOrbits
        {
            get => ellipseOrbits;
            set => ellipseOrbits = value;
        }

        void Start()
        {
            spoonController.OnPositionChanged += OnPositionChanged;
        }

        void OnDestroy()
        {
            spoonController.OnPositionChanged -= OnPositionChanged;
        }

        void OnPositionChanged(float value)
        {
            float angleDelta = value * speedMultiplier;

            int count = ellipseOrbits.Count;

            for (int i = 0; i < count; i++)
            {
                ellipseOrbits[i].AddAngle(angleDelta);
            }
        }
    }
}