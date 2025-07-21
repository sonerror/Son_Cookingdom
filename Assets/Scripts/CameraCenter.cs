
using DG.Tweening;
using UnityEngine;
namespace sonnv
{
    public class CameraCenter : SonMonoBehaviour
    {
        public Camera Cam => cam;
        private Camera cam;
        static float size0 = 5f;
        static float size1 = 5f;
        static float size2 = 6f;
        static float size3 = 7f;
        static float size4 = 8f;

        public bool EnableResizeCam = true;
        public bool EnableChangeCamPos = true;
        public bool isActiveUpdateCameSize = true;
        private float currentSize;

        private Vector3 pos1 = new Vector3(0, 0, -10);
        private Vector3 pos2 = new Vector3(0, 0, -10);

        void Awake()
        {
            cam = GetComponent<Camera>();
            currentSize = cam.orthographicSize;
        }

        void Start()
        {
            ResizeCam();
        }


        void Update()
        {
            if (isActiveUpdateCameSize && EnableResizeCam)
            {
                ResizeCam();
            }
        }

        void setCameraSize(float ortho)
        {
            if (ortho == currentSize)
            {
                return;
            }
            currentSize = ortho;
            cam.orthographicSize = ortho;
        }

        void setCamPos(Vector3 pos)
        {
            if (!EnableChangeCamPos) return;
            Tf.position = pos;
        }

        private void ResizeCam()
        {
            if (Screen.width == 344 && Screen.height == 882)
            {
                // Tf.position = pos1;
                setCamPos(pos1);
                setCameraSize(size4);
            }
            else if (Screen.width == 650 && Screen.height == 1396)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 414 && Screen.height == 896)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 896 && Screen.height == 414)
            {
                setCamPos(pos2);
                setCameraSize(size0);
            }
            else if (Screen.width == 360 && Screen.height == 740)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 430 && Screen.height == 932)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 390 && Screen.height == 844)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 416 && Screen.height == 896)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 412 && Screen.height == 915)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 412 && Screen.height == 914)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 900 && Screen.height == 1950)
            {
                setCamPos(pos1);
                setCameraSize(size2);
            }
            else if (Screen.width == 1950 && Screen.height == 900)
            {
                setCamPos(pos2);
                setCameraSize(size0);
            }
            else if (Screen.width == 320 && Screen.height == 622)
            {
                setCamPos(pos1);
                setCameraSize(size1);
            }
            else if (Screen.width == 622 && Screen.height == 320)
            {
                setCamPos(pos2);
                setCameraSize(size0);
            }
            else if (Screen.width == 325 && Screen.height == 698)
            {
                setCamPos(pos1);
                setCameraSize(size3);
            }
            else if (Screen.width == 698 && Screen.height == 325)
            {
                setCamPos(pos2);
                setCameraSize(size0);
            }
            else if (Screen.width == 1080 && Screen.height == 1920)
            {
                setCameraSize(size1);
                setCamPos(pos1);

            }
            else if (Screen.width < Screen.height)
            {
                setCamPos(pos1);
                setCameraSize(size1);
            }
            else if (Screen.width > Screen.height)
            {
                setCamPos(pos2);
                setCameraSize(size0);
            }
        }
    }

}
