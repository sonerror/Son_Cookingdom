using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Satisgame;
using sonnv;
using UnityEngine;

public class Level630 : SonLevelBase
{
    [SerializeField] private List<ShowItem> itemsShowState0 = new List<ShowItem>();
    [SerializeField] private List<ShowItem> itemsShowState1 = new List<ShowItem>();

    [SerializeField] private ParticleSystem parHeart;
    [SerializeField] private EmojiControl emojiControl;
    [SerializeField] private Transform capybaraTf;

    [SerializeField] private GameObject Scene1;
    [SerializeField] private GameObject Scene2;

    private int CountItemState0 = 0;
    public void OnItemState0Done()
    {
        CountItemState0++;
        if (CountItemState0 == 5)
            OnNextState();
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
        }
    }

    public void PlayEmojiHeart()
    {
        emojiControl.ShowPositive();
    }



}
