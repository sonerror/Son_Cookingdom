using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace sonnv
{
    public class ItemMortar : MonoBehaviour
    {
        [SerializeField] private GameObject objOld;
        [SerializeField] private SpriteRenderer objnew;
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private FlourMoveToCream moveObject;

        private bool isPlayEffect = false;
        public void OnPlayEffect()
        {
            if (isPlayEffect == false)
            {
                objOld.SetActive(false);
                objnew.gameObject.SetActive(true);
                particleSystem.Play();
                isPlayEffect = true;
            }
        }
        public void OnPlayMove()
        {
            objnew.maskInteraction = SpriteMaskInteraction.None;
            moveObject.OnMove();
        }

    }
}
