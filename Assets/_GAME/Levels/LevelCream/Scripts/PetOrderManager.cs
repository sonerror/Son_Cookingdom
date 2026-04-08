using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class PetOrderManager : Singleton<PetOrderManager>
    {
        [SerializeField] private List<PetOrder> _listPetOrder = new List<PetOrder>();
        public List<PetOrder> ListPetOrder => _listPetOrder;
        [SerializeField] private PetOrder petOrder;
        public void OnShowOrder()
        {
            for (int i = 0; i < _listPetOrder.Count; i++)
            {
                if (!_listPetOrder[i].IsShowPet)
                {
                    petOrder = _listPetOrder[i];
                    petOrder.OnShow();
                    return;
                }
            }
        }

        public void OnHideOrder()
        {
            if (petOrder != null)
            {
                petOrder.OnHide();
                petOrder = null;
            }
        }

        public void ResetAll()
        {
            for (int i = 0; i < _listPetOrder.Count; i++)
            {
                _listPetOrder[i].OnHide();
            }
            petOrder = null;
        }
        public void OnDoneOrder()
        {
            petOrder.OnHide();
        }
    }
}
