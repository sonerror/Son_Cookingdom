using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class LockObjectInsideScreen : SonMonoBehaviour
    {
        [SerializeField] private float topOffset;
        [SerializeField] private float botOffset;
        [SerializeField] private float leftOffset;
        [SerializeField] private float rightOffset;

        [SerializeField] private float delayDetect = 1f;

        private float _xMin;
        private float _xMax;
        private float _yMin;
        private float _yMax;
        private Camera _mainCam;

        private void Awake()
        {
            _mainCam = Camera.main;
        }

        private void Start()
        {
            Oninit();
        }
        public void Oninit()
        {
            _xMin = _mainCam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + leftOffset;
            _xMax = _mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - rightOffset;
            _yMin = _mainCam.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + botOffset;
            _yMax = _mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - topOffset;
        }
        private void LateUpdate()
        {
            if (delayDetect > 0)
            {
                delayDetect -= Time.deltaTime;
                return;
            }

            Vector3 pos = Tf.position;
            pos.x = Mathf.Clamp(pos.x, _xMin, _xMax);
            pos.y = Mathf.Clamp(pos.y, _yMin, _yMax);
            Tf.position = pos;
        }
    }

}
