using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.CookV2
{
    public class FruitPreparationStep : MonoBehaviour
    {
        public FruitPreparation fruitPreparation;
        
        public void OnComplete()
        {
            fruitPreparation.OnDoneStep();
        }
        public virtual void Setup(FruitPreparation preparation)
        {
            this.fruitPreparation = preparation;
        }
    }
}
