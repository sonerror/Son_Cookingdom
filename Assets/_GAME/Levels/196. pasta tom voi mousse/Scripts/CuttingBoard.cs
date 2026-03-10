using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    [Serializable]
    public class NSnapObjectToCuttingObject
    {
        public SonSnapObject itemDrag;
        public CuttingObject itemCut;
        public UnityEvent onEnableCuttingObject;
    }
    public class CuttingBoard : Singleton<CuttingBoard>
    {
        [SerializeField] private SonSnapPoint snapPointKnife;
        [SerializeField] private SonSnapPoint inforTFTarget;
        public SonSnapPoint InforTFTarget => inforTFTarget;
        [SerializeField] private KnifeCut knifeCut;
        public KnifeCut KnifeCut => knifeCut;
        [SerializeField] private List<NSnapObjectToCuttingObject> listSnapObjects;
        [SerializeField] private CuttingObject _currentCuttingCut;
        [SerializeField] private bool _canCut;

        public CuttingObject CurrentCuttingObject => _currentCuttingCut;
        public bool CanCut => _canCut;
        [SerializeField] private List<InforTFTarget> listTarget;
        private InforTFTarget GetFirstSnapTarget()
        {
            for (int i = 0; i < listTarget.Count; i++)
            {
                if (listTarget[i].IsSnap)
                {
                    return listTarget[i];
                }
            }
            return null;
        }
        public void RegisterSnapDone(SonSnapObject moveItem)
        {
            for (int i = 0; i < listSnapObjects.Count; i++)
            {
                NSnapObjectToCuttingObject data = listSnapObjects[i];

                if (data.itemDrag == moveItem)
                {
                    _currentCuttingCut = data.itemCut;
                    _currentCuttingCut.SetUp();
                    _canCut = _currentCuttingCut != null;
                    if (_canCut)
                    {
                        knifeCut.SetDataObjectCut(_currentCuttingCut);
                    }
                    _currentCuttingCut.SetDataTargetMove(GetFirstSnapTarget());
                    data.onEnableCuttingObject?.Invoke();
                    return;
                }
            }
        }
        public void ResetInforTFTarget()
        {
            inforTFTarget.ChangeCanSnap(true);
            inforTFTarget.ForceChangeSnap(false);
        }
        public void ResetCanSnap()
        {
            snapPointKnife.ChangeCanSnap(true);
            snapPointKnife.ForceChangeSnap(false);
        }
        public void ResetCutting()
        {
            if (_currentCuttingCut != null)
            {
                _currentCuttingCut.gameObject.SetActive(false);
                _currentCuttingCut = null;
            }

            _canCut = false;
        }
    }
}