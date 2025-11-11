using AnhPD.CookV2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Tanghulu
{
    public class TanghuluS1Phase4 : APDProgressionPhase
    {
        [SerializeField] private APDv2Drag sesameDrag, almondDrag;
        [SerializeField] private TanghuluNutPot nutPot;
        [SerializeField] private TanghuluNutSpoon spoon;
        [SerializeField] private TanghuluNutSpoon sesameSpoon;
        [SerializeField] private TanghuluMortar mortar;
        [SerializeField] private APDv2Drag mortarDrag;
        [SerializeField] private APDv2Drag pestleDrag;
        [SerializeField] private APDv2Drag salt;
        protected override void Setup()
        {
            base.Setup();
            
            almondDrag.OnReady();
            almondDrag.SetCondition(()=>!nutPot.isHaveObject);
            almondDrag.onComplete.AddListener(()=>
            {
                nutPot.OnPutAlmondIn();
                spoon.gameObject.SetActive(true);
                sesameSpoon.gameObject.SetActive(false);
                
                SetHintAccordingToGroup(0);
            });

            sesameDrag.OnReady();
            sesameDrag.SetCondition(()=>!nutPot.isHaveObject);
            sesameDrag.onComplete.AddListener(() =>
            {
                nutPot.OnPutSesameIn();
                salt.OnReady();
                
                spoon.gameObject.SetActive(false);
                sesameSpoon.gameObject.SetActive(true);
                
                SetHintAccordingToGroup(1);
            });
            salt.onComplete.AddListener(nutPot.OnPutSaltIn);

            nutPot.OnAlmondReadyFlip += () => DoneStepText(1);
            nutPot.OnAllAlmondFlipped += ()=> DoneStepImageOfGroup(0);
            nutPot.OnAlmondCooked += spoon.DragMode;
            
            nutPot.OnSesameCooked += ()=>
            {
                sesameSpoon.OnReady();
                DoneStepText(3);
            };
            nutPot.onTurnOff.AddListener(OnTurnOffStove);

            spoon.OnScoop += nutPot.EmptyPot;
            spoon.OnReturnObject += ()=> nutPot.OnNutBack(true);
            spoon.onComplete.AddListener(()=> 
            {
                mortar.PutInNut();
                pestleDrag.OnReady();
            });
            
            sesameSpoon.OnScoop += nutPot.EmptyPot;
            sesameSpoon.OnReturnObject += ()=> nutPot.OnNutBack(true);
            sesameSpoon.onComplete.AddListener(() =>
            {
                _isDoneSesame = true;
                DoneStepImageOfGroup(1);
                DoneStepText(4);
                CheckComplete();
            });
            
            pestleDrag.onComplete.AddListener(mortar.PutInPestle);
            mortar.OnDone += () =>
            {
                pestleDrag.ReturnToStartPos();
                mortarDrag.gameObject.SetActive(true);
                mortar.gameObject.SetActive(false);
            };
            
            mortarDrag.onComplete.AddListener(() =>
            {
                _isDoneAlmond = true;
                DoneStepImageOfGroup(0);
                DoneStepText(2);
                CheckComplete();
            });
        }

        private bool _isTurnOffStove, _isDoneAlmond, _isDoneSesame;
        private void OnTurnOffStove()
        {
            _isTurnOffStove = true;
            DoneStepImageOfGroup(2);
            DoneStepText(5);
            CheckComplete();
        }

        private void CheckComplete()
        {
            if (_isTurnOffStove && _isDoneAlmond && _isDoneSesame)
            {
                OnComplete();
            }
        }
    }
}
