using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AnhPD.KingCrab;
using UnityEngine;

public class MaskGroupCustom : MonoBehaviour
{
    [SerializeField] private List<GameObject> maskLeft;
    [SerializeField] private List<GameObject> maskRight;

    private void Start()
    {
        foreach (var mask in maskLeft)
        {
            mask.SetActive(false);
        }
        foreach (var mask in maskRight)
        {
            mask.SetActive(false);
        }
    }

    public void CheckMaskLeft(Vector3 pos)
    {
        foreach (var mask in maskLeft)
        {
            if (mask.activeSelf)
            {
                continue;
            }

            if (Vector2.Distance(pos, mask.transform.position) < 0.5f)
            {
                mask.SetActive(true);
            }
        }

        CheckDoneTrimMeat();
    }

    public void CheckMaskRight(Vector3 pos)
    {
        foreach (var mask in maskRight)
        {
            if (mask.activeSelf)
            {
                continue;
            }

            if (Vector2.Distance(pos, mask.transform.position) < 0.5f)
            {
                mask.SetActive(true);
            }
        }

        CheckDoneTrimMeat();
    }

    private void CheckDoneTrimMeat()
    {

        if (!maskLeft.All(x => x.activeSelf)) return;
        if (!maskRight.All(x => x.activeSelf)) return;

        LevelKingCrab.Instance.OnCompleteTrimMeat();
    }
}
