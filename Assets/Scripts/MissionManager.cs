using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private int targetTaskCount;
    private int _currentTaskCount;

    [SerializeField] private List<UnityEvent> onComplete;

    public void AddCompleteTask()
    {
        _currentTaskCount++;
        
        CheckCompleteAllTasks();
    }

    private void CheckCompleteAllTasks()
    {
        if (_currentTaskCount != targetTaskCount) return;
        foreach (var onCompleteEvent in onComplete)
        {
            onCompleteEvent.Invoke();
        }
    }
}
