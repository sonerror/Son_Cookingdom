using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.FoodStall
{
    [CreateAssetMenu(menuName = "AnhPD/FoodStall/ColorTagSO")]
    public class ColorTagSO : ScriptableObject
    {
        public List<ColorTagConfig> colorTags;
        
        /// <summary>
        /// Tìm ColorTagConfig theo nameId
        /// </summary>
        public ColorTagConfig GetConfig(string nameId)
        {
            return colorTags.Find(c => c.nameId == nameId);
        }

        /// <summary>
        /// Tìm ColorTag list theo nameId
        /// </summary>
        public List<ColorTag> GetColorTags(string nameId)
        {
            var config = GetConfig(nameId);
            return config?.colorTags;
        }
    }

    [Serializable]
    public class ColorTagConfig
    {
        public string nameId;
        public List<ColorTag> colorTags = new List<ColorTag>();
    }
}
