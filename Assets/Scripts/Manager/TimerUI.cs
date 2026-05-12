using UnityEngine;
using TMPro;
using System.Collections;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTimer;
    [SerializeField] private AudioClip sfxTick;

    private Coroutine _corTimer;
    private bool _hasPlayedWarning = false;

    private void Start()
    {
        //StartCountdown();
    }

    public void StartCountdown()
    {
        _hasPlayedWarning = false;
        if (_corTimer != null) StopCoroutine(_corTimer);
        _corTimer = StartCoroutine(IECountdown());
    }

    public void StopCountdown()
    {
        if (_corTimer != null) StopCoroutine(_corTimer);
    }

    private IEnumerator IECountdown()
    {
        while (!TimerManager.Ins.IsFinished)
        {
            UpdateUI();
            PlayTickSound();
            yield return new WaitForSecondsRealtime(1f);
            TimerManager.Ins.Tick();
        }

        UpdateUI();
        OnTimeUp();
    }

    private void UpdateUI()
    {
        if (txtTimer == null) return;
        int time = TimerManager.Ins.TimeCount;
        int minutes = time / 60;
        int seconds = time % 60;
        txtTimer.text = $"{minutes:00}:{seconds:00}";
        txtTimer.color = time <= 10 ? Color.red : Color.white;
    }

    private void PlayTickSound()
    {
        if (TimerManager.Ins.TimeCount <= 10)
        {
            SoundManager.PlaySFXOneShot(sfxTick);
        }
    }

    private void OnTimeUp()
    {
        if (TimerManager.Ins.CheckLose())
        {
            UpdateUI();
            Debug.Log("LOSE - Hết giờ!");
            UIManager.Instance.CloseUIGamePlay();
            UIManager.Instance.LoadUILose();
            GameManager.Ins.showEndGame();
        }
    }
}
