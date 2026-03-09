using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{

    [Serializable]
    public class PointInPlateEnd
    {
        public SpriteRenderer spriteRenderer;
        public Transform tf;
        public bool isAdd;
    }
    [Serializable]
    public class PointInPlate
    {
        public Transform tf;
        public bool isAdd;
    }
    public class DonutForwarder : IngredientForwarder
    {
        [SerializeField] private float moveDistance = 0.15f;
        [SerializeField] private float moveDuration = 0.4f;
        [SerializeField] private float rotationAngle = 10f;
        [SerializeField] private float rotationDuration = 0.4f;
        [SerializeField] private SpriteRenderer donutOri;
        [SerializeField] private List<IngredientRotate> listDonut;
        [SerializeField] private IngredientType donutColor;
        [SerializeField] private List<IngredientEnd> listDonutEnd;

        private Vector3 startPos;
        private Quaternion startRot;


        public bool isAddedToPlate;
        [SerializeField] private ScalingOnPick scalingOnPick;
        protected override void Awake()
        { 
            base.Awake();
            isAddedToPlate = false;
        }

        public void SetIsClick(bool _b)
        {
            scalingOnPick.SetIsClick(_b);
        }


        public void SetDonutColor(IngredientType color)
        {
            donutColor = color;
        }

        public IngredientType GetDonutColor()
        {
            return donutColor;
        }
        public void StartPunk()
        {
            startPos = transform.localPosition;
            startRot = transform.localRotation;
            Sequence seq = DOTween.Sequence();
            seq.Insert(0f, transform.DOScale(0.85f, moveDuration).SetEase(Ease.InOutBack));
            seq.Append(transform.DOLocalMoveY(startPos.y - moveDistance, moveDuration).SetEase(Ease.InSine));
            seq.Join(transform.DOLocalRotate(new Vector3(0, 0, rotationAngle), rotationDuration).SetEase(Ease.InOutSine));
            seq.Append(transform.DOLocalMoveY(startPos.y, moveDuration).SetEase(Ease.OutSine));
            seq.Join(transform.DOLocalRotate(Vector3.zero, rotationDuration).SetEase(Ease.InOutSine));
            seq.Append(transform.DOScale(1f, moveDuration * 0.8f).SetEase(Ease.OutBack));
            isAddedToPlate = true;
        }


        public SpriteRenderer SetImgByType(IngredientType type)
        {
            return listDonut.Find(t => t.ingredientType == type).sprite;
        }
        public void HideDonutOrig(bool b)
        {
            donutOri.enabled = b;
        }
        public void SetTypeIngre()
        {
            ingredient = IngredientType;
        }
        public void ChangeDonutOri(Sprite s)
        {
            HideDonutOrig(true);
            donutOri.sprite = s;
        }

        public Sprite GetDonutEndByType(IngredientType type)
        {
            return listDonutEnd.Find(t => t.ingredientType == type).sprite;
        }
    }
}
