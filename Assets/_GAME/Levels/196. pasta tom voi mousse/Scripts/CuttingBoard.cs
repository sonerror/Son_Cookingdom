using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    [Serializable]
    public class NSnapObjectToCuttingObject
    {
        public FlourMoveToCream itemMove;
        public CuttingObject itemCut;
        public UnityEvent onEnableCuttingObject;
    }

    public class CuttingBoard : Singleton<CuttingBoard>
    {
        [SerializeField] private SonSnapPoint snapPointKnife;
        [SerializeField] private InforTFTarget inforTFTarget;
        public InforTFTarget InforTFTarget => inforTFTarget;
        [SerializeField] private KnifeCut knifeCut;
        public KnifeCut KnifeCut => knifeCut;
        [SerializeField] private List<NSnapObjectToCuttingObject> listSnapObjects;

        [SerializeField] private CuttingObject _currentCuttingCut;

        [SerializeField] private bool _canCut;

        public CuttingObject CurrentCuttingObject => _currentCuttingCut;
        public bool CanCut => _canCut;

        public void RegisterMoveDone(FlourMoveToCream moveItem)
        {
            for (int i = 0; i < listSnapObjects.Count; i++)
            {
                NSnapObjectToCuttingObject data = listSnapObjects[i];

                if (data.itemMove == moveItem)
                {
                    _currentCuttingCut = data.itemCut;
                    _currentCuttingCut.SetUp();
                    _canCut = _currentCuttingCut != null;
                    TutorialManager.Ins.SetCanItemInBroad(true);
                    if (_canCut)
                    {
                        knifeCut.SetDataObjectCut(_currentCuttingCut);
                    }

                    data.onEnableCuttingObject?.Invoke();
                    return;
                }
            }
        }
        public void ResetInforTFTarget()
        {
            inforTFTarget.ChangeIsSnap(true);
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