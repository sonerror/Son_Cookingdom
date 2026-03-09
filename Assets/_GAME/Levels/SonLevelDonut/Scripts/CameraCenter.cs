
using UnityEngine;
namespace sonnv
{
    public class CameraCenter : SonMonoBehaviour
    {
        [SerializeField] private Camera cam;

        [SerializeField] private bool canScrollCam;
        // [SerializeField] private Trung_ScrollCam scrollCam;
        private float currentSizeCam;

        private float sizeTarget;
        private void Awake()
        {
            sizeTarget = cam.orthographicSize;
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = 9f / 16f;

            if (screenRatio >= targetRatio)
            {
                cam.orthographicSize = sizeTarget;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                cam.orthographicSize = sizeTarget * differenceInSize;
            }
        }
    }

}
