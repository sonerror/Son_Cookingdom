using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ItemClick : GameUnit, IItem
{
    private bool isDone = false;
    public bool IsDone { get => isDone; private set => isDone = value; }
    public Collider Col => col;

    [SerializeField] private Animator anim;
    [SerializeField] private Collider col;

    public void OnActionEnd()
    {
        throw new System.NotImplementedException();
    }

    public void OnActionStart()
    {
        throw new System.NotImplementedException();
    }

    void OnMouseDown()
    {
        TutorialManager.Ins.MouseDownItem();
        Col.enabled = false; // Disable collider to prevent further clicks
        PlayAction(); // Call the method to perform the action on click
    }

    private void OnMouseUp()
    {
        TutorialManager.Ins.MouseUpItem();
    }

    void PlayAction()
    {
        anim.SetTrigger("Play");
        DelayCallDone();
        StartCoroutine(IEPlayAction());
    }

    IEnumerator IEPlayAction()
    {
        yield return Cache.GetWFS(0.45f);
        for (int i = 0; i < 3; i++)
        {
            PoolManager.Ins.Spawn(PoolType.SfxClose, Tf.position, Quaternion.identity);
            yield return Cache.GetWFS(0.4f);
        }
    }

    private async void DelayCallDone()
    {
        await Task.Delay(2000);
        IsDone = false;
        var level = LevelBase.Ins as Level630;
        level.OnItemState0Done();
        // level.PlayEmojiHeart();
    }
}
