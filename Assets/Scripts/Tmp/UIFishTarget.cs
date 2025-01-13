using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AnhPD.Fishing
{
    public class UIFishTarget : MonoBehaviour
    {
        [SerializeField] private UIFishTargetDisplay display;
        [SerializeField] private TextMeshProUGUI txtNumber;
        [SerializeField] private GameObject tick, icon;
        
        public Fish.ColorType FishType;
        public bool IsComplete => count <= 0;
        public bool IsActive { get; private set; }

        private int count = 0;

        private void Start()
        {
            gameObject.SetActive(false);
            icon.SetActive(false);
            txtNumber.gameObject.SetActive(false);
            tick.SetActive(false);
        }

        public void AddThisTarget()
        {
            if (count == 0)
            {
                IsActive = true;
                gameObject.SetActive (true);
                transform.position = display.GetPositon();
                icon.SetActive(true);
                txtNumber.gameObject.SetActive(true);
            }
            count++;
            txtNumber.text = count.ToString();
        }
        public void OnCatched()
        {
            if(count > 0)
            {
                count--;
                txtNumber.text = count.ToString();
                if (count == 0)
                {
                    StartCoroutine(delayTurnOff());
                }
                IEnumerator delayTurnOff()
                {
                    yield return new WaitForSeconds(.3f);
                    tick.SetActive(true);
                    txtNumber.text = "";
                }
            }

        }
    }
}

