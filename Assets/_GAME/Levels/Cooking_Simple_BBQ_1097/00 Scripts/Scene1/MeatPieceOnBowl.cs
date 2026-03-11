using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
namespace sonnv
{
    public class MeatPieceOnBowl : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer rawSprRdr, mixedSprRdr;
        [SerializeField] private ActionAnim actionIn;



        public void OnFadeIn(float time)
        {
            FadeSprite(rawSprRdr, 1f, time, Ease.Linear, actionIn.OnActive);
        }
        void FadeSprite(SpriteRenderer sprite, float alpha, float time, Ease ease = Ease.Linear, System.Action onDone = null)
        {
            sprite.DOFade(1, time)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    rawSprRdr.SetAlpha(0);
                    mixedSprRdr.SetAlpha(1);
                    onDone?.Invoke();
                });

        }
        public void ChangeAlphaMixed(float alpha)
        {
            rawSprRdr.SetAlpha(1f - alpha);
            mixedSprRdr.SetAlpha(alpha);
        }
    }
}
