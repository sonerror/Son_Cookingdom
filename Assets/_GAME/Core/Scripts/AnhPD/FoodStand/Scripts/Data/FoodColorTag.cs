using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.FoodStall;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class FoodColorTag : MonoBehaviour
    {
        [SerializeField] protected ColorTagSO config;
        
        [SerializeField] protected List<ColorTag> mainIngredientTag;
        [SerializeField] protected List<ColorTag> subIngredientTag;
        [SerializeField] private List<ExclusiveTagGroup> exclusiveGroups = new List<ExclusiveTagGroup>();

        [ShowInInspector, ReadOnly] protected List<ColorTag> positiveTags = new List<ColorTag>();
        [ShowInInspector, ReadOnly] protected List<ColorTag> negativeTags = new List<ColorTag>();
        [ShowInInspector, ReadOnly] protected ColorTag mainTag;
        [ShowInInspector, ReadOnly]protected List<ColorTag> tags = new List<ColorTag>();
        
        public ColorTag MainTag => mainTag;
        public List<ColorTag> PositiveTags => positiveTags;
        public List<ColorTag> NegativeTags => negativeTags;
        
        public virtual void Random()
        {
            mainTag = mainIngredientTag[UnityEngine.Random.Range(0, mainIngredientTag.Count)];

            positiveTags.Clear();
            negativeTags.Clear();

            if (subIngredientTag.Count > 0)
            {
                int positiveCount = UnityEngine.Random.Range(2, 5);
                positiveCount = Mathf.Min(positiveCount, subIngredientTag.Count);

                List<ColorTag> pool = new List<ColorTag>(subIngredientTag);

                for (int i = 0; i < positiveCount && pool.Count > 0; i++)
                {
                    int idx = UnityEngine.Random.Range(0, pool.Count);
                    ColorTag chosen = pool[idx];
                    positiveTags.Add(chosen);
                    pool.RemoveAt(idx);

                    // kiểm tra chosen có nằm trong group nào không
                    foreach (var group in exclusiveGroups)
                    {
                        if (group.tags.Contains(chosen))
                        {
                            // loại bỏ các tag khác trong group
                            pool.RemoveAll(t => group.tags.Contains(t) && !t.Equals(chosen));
                        }
                    }
                }

                // random 0–1 negative
                int negativeCount = UnityEngine.Random.Range(0, 2);
                if (negativeCount > 0 && pool.Count > 0)
                {
                    int idx = UnityEngine.Random.Range(0, pool.Count);
                    ColorTag candidate = pool[idx];

                    // đảm bảo không trùng positive
                    if (!positiveTags.Contains(candidate))
                    {
                        negativeTags.Add(candidate);
                    }

                    pool.RemoveAt(idx);
                }
            }
        }

        public void SetMainTag(string nameId)
        {
            mainTag = config.GetColorTags(nameId)[0];
        }
        public void AddSubTags(string nameId)
        {
            List<ColorTag> colorTags = config.GetColorTags(nameId);
            foreach (ColorTag colorTag in colorTags)
            {
                if (!tags.Contains(colorTag)) tags.Add(colorTag);
            }
        }

        public bool IsSatisfiedTag(FoodColorTag other)
        {
            // 1. mainTag phải trùng
            if (mainTag != other.mainTag)
            {
                Debug.Log("main");
                return false;
            }

            // 2. other phải chứa tất cả positiveTags 
            foreach (var pos in positiveTags)
            {
                if (!other.tags.Contains(pos))
                {
                    Debug.Log("positive");
                    return false;
                }
            }

            // 3. other không được chứa tất cả negativeTags 
            foreach (var neg in negativeTags)
            {
                if (other.tags.Contains(neg))
                {
                    Debug.Log("negative");
                    return false;
                }
            }

            return true;
        }
        public void Clear()
        {
            tags.Clear();
            mainTag = ColorTag.None;
        }
    }
    [System.Serializable]
    public class ExclusiveTagGroup
    {
        public List<ColorTag> tags = new List<ColorTag>();
    }
}
