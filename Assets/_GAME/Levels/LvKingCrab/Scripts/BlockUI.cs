// using Satisdy.PetGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Utilities;

namespace AnhPD
{
    public class BlockUI : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        // private LevelBase _level;

        private Vector2 _sizeDelta;
        private void Awake()
        {
            // save the rect transform size
            if (!rect)
            {
                rect = GetComponent<RectTransform>();
            }
            _sizeDelta = rect.sizeDelta;
        }

        private void Start()
        {
            // _level = LevelBase.instance;
            // if (_level)
            // {
            //     _level.onBlockPlayerInteractChanged += BlockInput;
            // }
        }

        // private void BlockInput()
        // {
        //     if (!_level.IsAllowInteract)
        //     {
        //         // change rect size to bound full screen
        //         rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        //     }
        //     else
        //     {
        //         // restore the rect size
        //         this.WaitOneFrame(() => rect.sizeDelta = _sizeDelta);
        //     }
        // }

        // private void OnDestroy()
        // {
        //     if (_level)
        //     {
        //         _level.onBlockPlayerInteractChanged -= BlockInput;
        //     }
        // }
    }
}

