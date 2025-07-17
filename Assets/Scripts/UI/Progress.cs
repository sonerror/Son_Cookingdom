using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Progress : MonoBehaviour
{
    // public TextMeshProUGUI txtProgressValue;
    public Image greenPanel;
    public float greenPanelFillAmount = 0;
    public int totalProgress = 100;
    public int currProgress = 0;
    public bool isReduceProgress = false;
    public float speedFloatingIdle = 0.5f;


    private void Start()
    {
        setProgerss(currProgress);
    }

    public void setFullProgress()
    {
        setProgerss(totalProgress);
        greenPanel.fillAmount = 1;
    }

    public void setEmptyProgress()
    {
        setProgerss(0);
        greenPanel.fillAmount = 0;
    }

    public virtual void increaseProgress(int from, int to)
    {
        setProgerss(currProgress + (int)Random.Range(from, to));
    }

    public void reduceProgress(int from, int to)
    {
        var a = (int)Random.Range(from, to);
        setProgerss(currProgress - a);
    }

    public void setProgerss(int progress)
    {
        // Debug.Log("setProgerss: " + progress);
        currProgress = progress;
        // txtProgressValue.text = currProgress.ToString();
        greenPanelFillAmount = (float)currProgress / totalProgress;
    }

    private void Update()
    {
        FillAmount();
    }

    public virtual void FillAmount()
    {
        if (isReduceProgress)
        {
            if (greenPanel.fillAmount > greenPanelFillAmount)
            {
                greenPanel.fillAmount -= greenPanelFillAmount * Time.deltaTime * speedFloatingIdle;
            }
        }
        else
        {
            if (greenPanel.fillAmount < greenPanelFillAmount)
            {
                greenPanel.fillAmount += greenPanelFillAmount * Time.deltaTime * speedFloatingIdle;
            }
        }
    }
}