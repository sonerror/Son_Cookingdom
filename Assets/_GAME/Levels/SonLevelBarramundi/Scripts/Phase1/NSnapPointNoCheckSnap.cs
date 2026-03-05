namespace sonnv
{
    public class NSnapPointNoCheckSnap : SonSnapPoint
    {
        public override void OnSnap()
        {
            isSnap = false;
        }
    }
}