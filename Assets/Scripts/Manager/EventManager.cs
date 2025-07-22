using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public enum EventType
{
    IncreaseProgress = 1,
    IncreaseProgressScaled = 2,

}

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;
    private List<UnityEvent> eventStack = new List<UnityEvent>();

    private static EventManager m_Ins;

    public static EventManager Ins
    {
        get
        {
            return m_Ins;
        }
    }


    private void Awake()
    {
        if (m_Ins != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            m_Ins = this;
            m_Ins.Init();
            DontDestroyOnLoad(gameObject);
        }
    }


    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        if (Ins == null) return;
        UnityEvent thisEvent = null;
        if (Ins.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Ins.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (m_Ins == null) return;
        UnityEvent thisEvent = null;
        if (Ins.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Ins)
        {
            if (Ins.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
    public static void AddEventNextFrame(UnityAction listener)
    {
        UnityEvent thisEvent = new UnityEvent();
        thisEvent.AddListener(listener);
        Ins.eventStack.Add(thisEvent);
    }
    private void Update()
    {
        while (Ins.eventStack.Count > 0)
        {
            UnityEvent thisEvent = Ins.eventStack[0];
            thisEvent.Invoke();
            Ins.eventStack.RemoveAt(0);
        }
    }
}
