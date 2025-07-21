using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Satisgame;
using sonnv;
using UnityEngine;

public class Level630 : SonLevelBase
{
    public static bool IsState2 = false;

    [SerializeField] private List<ShowItem> itemsShowState0 = new List<ShowItem>();
    [SerializeField] private List<ShowItem> itemsShowState1 = new List<ShowItem>();

    [SerializeField] private ParticleSystem parHeart;
    [SerializeField] private EmojiControl emojiControl;
    [SerializeField] private Transform capybaraTf;

    [SerializeField] private GameObject Scene1;
    [SerializeField] private GameObject Scene2;
    public List<ItemMove> itemState1 = new List<ItemMove>();
    [SerializeField] public ItemClick itemClick;
    [SerializeField] public ItemHolder itemHolderBroad;
    [SerializeField] public Knife knife;

    public List<ItemMoveState2> itemState2 = new List<ItemMoveState2>();

    private int CountItemState0 = 0;
    public void OnItemState0Done()
    {
        CountItemState0++;
        TutorialManager.Ins.MouseUpItem();
        if (CountItemState0 == 5)
            OnNextState();
    }

    void OnItemState1Done()
    {
        CountItemState0++;
        if (CountItemState0 >= 8)
        {
            GameManager.Ins.ActiveListenToStore();
        }
    }

    void OnNextState()
    {
        StartCoroutine(IE_ChangeState());
    }

    IEnumerator IE_ChangeState()
    {
        capybaraTf.gameObject.SetActive(true);
        var pos = capybaraTf.localPosition;
        capybaraTf.localPosition = pos + Vector3.down * 1.85f;
        capybaraTf.DOLocalMove(pos, 1f);
        yield return Cache.GetWFS(0.5f);
        SoundManager.Ins.PlayFx(FxType.EmojiPositive);
        parHeart.Play();
        yield return Cache.GetWFS(1f);

        TutorialManager.Ins.MouseUpItem();///////////////////

        capybaraTf.gameObject.SetActive(false);
        for (var i = 0; i < itemsShowState0.Count; i++)
        {
            itemsShowState0[i].MoveObjHide();
        }
        yield return Cache.GetWFS(1.5f);
        Scene1.SetActive(false);
        Scene2.SetActive(true);
        for (var i = 0; i < itemsShowState1.Count; i++)
        {
            itemsShowState1[i].MoveObjShow();
            yield return Cache.GetWFS(0.1f);
        }
        InitState2();
    }

    void InitState2()
    {


        for (var i = 0; i < itemState2.Count; i++)
        {
            itemState2[i].IsEnableCheck = false;
        }
        IsState2 = true;
        itemState2[0].IsEnableCheck = true;
    }

    public void PlayEmojiHeart()
    {
        emojiControl.ShowPositive();
    }

    public void NextStepInState2()
    {
        OnItemState1Done();
        for (var i = 0; i < itemState2.Count; i++)
        {
            if (itemState2[i].gameObject.activeSelf)
            {
                itemState2[i].IsEnableCheck = true;
                return;
            }
        }
    }

}
