using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    [SerializeField] private int timeCount;
    public int TimeCount => timeCount;

    [SerializeField] private int timeTarget;
    public int TimeTarget => timeTarget;

    public bool IsFinished { get; private set; } = false;

    public void Tick()
    {
        if (IsFinished) return;
        timeCount--;
        if (timeCount <= 0)
        {
            timeCount = 0;
            IsFinished = true;
        }
    }

    public bool CheckWin() => timeCount >= timeTarget;
    public bool CheckLose() => timeCount <= 0;
}
