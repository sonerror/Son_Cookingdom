using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class StuffAmount : MonoBehaviour
    {
        [SerializeField] GameObject tool;
        [SerializeField] GameObject[] masks;
        int index = 0;

        public void OnTakeOffStuff()
        {
            masks[index].SetActive(true);
        }

        public void OnPutInStuff(bool isRemoved)
        {
            if (isRemoved)
            {
                index++;
                if(index >= masks.Length)
                {
                    tool.GetComponent<Collider2D>().enabled = false;
                    Destroy(this);
                }
            }
            else
            {
                if(index < masks.Length)
                {
                    masks[index].SetActive(false);
                }
            }

        }
    }
}

