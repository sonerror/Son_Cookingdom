using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScreen : MonoBehaviour
{
    public Transform btnPlay;
    public Transform logo;

    private float gameWidth = 1080f;
    private float gameHeight = 1920f;

    private Vector3 btnPlayPos = new Vector3(0, 0, 0);
    private Vector3 logoPos = new Vector3(0, 0, 0);

    private Vector3 scaleUp = new Vector3(1.5f, 1.5f, 1.5f);
    private Vector3 scaleDown = new Vector3(1f, 1f, 1f);


    private void Start()
    {
        logo.gameObject.SetActive(false);
    }

    public void gotoStore()
    {
        GameManager.Ins.gotoStore();
    }

    private bool isFirstClick = true;

    private void Update()
    {
        float ratio = (float)Screen.width / (float)Screen.height;
        // Debug.Log("ratio: " + ratio);
        float heightTmp = (float)1080f / ratio;
        float widthTmp = (float)1920f * ratio;
        // Debug.Log("heightTmp: " + heightTmp + " widthTmp: " + widthTmp);
        if (heightTmp <= 1920f)
        {
            gameWidth = widthTmp;
            gameHeight = 1920f;
        }
        else
        {
            gameHeight = 1920f;
            gameWidth = 1920f * ratio;
        }

        if (isFirstClick && Input.GetMouseButtonDown(0))
        {
            isFirstClick = false;
            logo.gameObject.SetActive(true);
        }

        // if (widthTmp < 1080f)
        // {
        //     gameWidth = 1080f;
        //     gameHeight = heightTmp;
        // }

        Debug.Log("gameWidth: " + gameWidth + " gameHeight: " + gameHeight);
        resizeUI();
    }

    private void resizeUI()
    {
        if (gameWidth > 1.2 * gameHeight)
        {
            btnPlayPos.Set(-540 - (gameWidth - 1080f) / 4, 0, 0);
            logoPos.Set(540 + (gameWidth - 1080f) / 4, 0, 0);

            btnPlay.localPosition = btnPlayPos;
            logo.localPosition = logoPos;

            btnPlay.localScale = scaleUp;
            logo.localScale = scaleUp;
        }
        else
        {
            btnPlayPos.Set(0, -150 + gameHeight / 2, 0);
            logoPos.Set(0, 100 - gameHeight / 2, 0);

            btnPlay.localPosition = btnPlayPos;
            logo.localPosition = logoPos;
            btnPlay.localScale = scaleDown;
            logo.localScale = scaleDown;
        }
    }
}
