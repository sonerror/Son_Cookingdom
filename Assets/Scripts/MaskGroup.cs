using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AnhPD.KingCrab;
using UnityEngine;

public class MaskGroup : MonoBehaviour
{
    [SerializeField] private List<GameObject> masks;
    private void Start()
    {
        foreach (var mask in masks)
        {
            mask.SetActive(false);
        }

    }

    public void CheckMask(Vector3 pos)
    {
        foreach (var mask in masks)
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

        if (!masks.All(x => x.activeSelf)) return;

        LevelKingCrab.Instance.OnCompleteTrimMeat2();
    }
}
