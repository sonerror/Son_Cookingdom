using DG.Tweening;
namespace sonnv
{
    public class ContactColider : SonMonoBehaviour
    {
        public void HideObj(float time)
        {
            this.gameObject.SetActive(true);
            DOVirtual.DelayedCall(time, () =>
            {
                this.gameObject.SetActive(false);
            });
        }
    }
}

