using System.Collections;
using System.Collections.Generic;
using AnhPD.FoodStall;
using UnityEngine;

namespace AnhPD.Tanghulu
{
    public class TanghuluData : FoodData
    {
        public List<FruitType> fruits;
        public CoatingType coatingType;
        public DecorType decorType;
        
        public List<DecorType> excludeDecorTypes = new List<DecorType>();
        public List<CoatingType> excludeCoatingTypes = new List<CoatingType>{ CoatingType.None};
        public enum FruitType
        {
            None = 0,
            PurpleGrape = 1,
            GreenGrape = 2,
            Tangerine = 3,
            Strawberry = 4,
            Kiwi = 5,
        }
        public enum CoatingType
        {
            None = 0,
            White = 1,
            Orange = 2,
            Brown = 3,
            Blue = 4,
        }
        public enum DecorType
        {
            None = 0,
            Almond = 1,
            Sesame = 2,
            BeanCandy = 3,
        }

        protected override bool Equals(FoodData other)
        {
            if (other is not TanghuluData o)
                return false;
            if(fruits.Count != o.fruits.Count) return false;
            
            for (int i = 0; i < fruits.Count; i++)
            {
                if(fruits[i] != o.fruits[i]) return false;
            }
            
            return coatingType == o.coatingType && decorType == o.decorType;
        }
        
        protected override void RandomData()
        {
            fruits.Clear();
            for (int i = 0; i < 5; i++)
            {
                fruits.Add(APDEnumUtilities.GetRandomEnumValue(new List<FruitType>{ FruitType.None }));
            }
            coatingType = APDEnumUtilities.GetRandomEnumValue(excludeCoatingTypes);
            decorType = APDEnumUtilities.GetRandomEnumValue<DecorType>(excludeDecorTypes);
        }

        public override void ResetData()
        {
            base.ResetData();
            fruits.Clear();
            coatingType = CoatingType.None;
            decorType = DecorType.None;
        }
    }
}
