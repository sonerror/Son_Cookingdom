using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class ColorShapeDisplay : DisplayBase
    {
        [SerializeField] private SpriteRenderer mainColorRender;
        [SerializeField] private SpriteRenderer[] positiveRender;
        [SerializeField] private SpriteRenderer[] negativeRender;
        [SerializeField] private Sprite[] colorSprites;
        [SerializeField] private ColorConfigSO config;
        public override void Display(FoodData foodData)
        {
            mainColorRender.gameObject.SetActive(true);
            mainColorRender.color = config.GetColor(foodData.colorTag.MainTag);
            List<ColorTag> positiveTags = foodData.colorTag.PositiveTags;
            
            float offset = 360f/positiveTags.Count;
            for (int i = 0; i < positiveTags.Count; i++)
            {
                SpriteRenderer ren = positiveRender[i];
                ren.transform.parent.gameObject.SetActive(true);
                ren.color = config.GetColor(positiveTags[i]);

                ren.sprite = positiveTags[i] == ColorTag.Rainbow ? colorSprites[1] : colorSprites[0];
                
                ren.transform.parent.localEulerAngles = new Vector3(0f, 0f, offset * i);
            }
            
            List<ColorTag> negativeTags = foodData.colorTag.NegativeTags;
            for (int i = 0; i < negativeTags.Count; i++)
            {
                SpriteRenderer ren = negativeRender[i];
                ren.gameObject.SetActive(true);
                ren.color = config.GetColor(negativeTags[i]);

                ren.sprite = negativeTags[i] == ColorTag.Rainbow ? colorSprites[1] : colorSprites[0];
            }
        }

        public override void ClearDisplay()
        {
           mainColorRender.gameObject.SetActive(false);
           foreach (var t in positiveRender) t.transform.parent.gameObject.SetActive(false);
           foreach (var t in negativeRender) t.gameObject.SetActive(false);
        }
    }
}
