using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] protected float taskTime = 5f;
    [SerializeField] protected float timeMultiplier = 1f;
    protected float _currentTime = 0f;

    [Space][SerializeField] protected UnityEvent OnStartTaskEvent;
    [Space][SerializeField] protected UnityEvent OnCompleteTaskEvent;
    public event System.Action<float, float> OnTaskTimeChanged;
    private Coroutine _taskCoroutine;

    private void OnMouseDown()
    {
        StartTask();
    }

    public virtual void StartTask()
    {
        if(_taskCoroutine == null)
        {
            _taskCoroutine = StartCoroutine(CalculateTaskTimer());
            OnStartTaskEvent?.Invoke();
        }
    }

    public virtual void FinishTask() => OnCompleteTaskEvent?.Invoke();

    private IEnumerator CalculateTaskTimer()
    {
        OnTaskTimeChanged?.Invoke(taskTime, timeMultiplier);
        _currentTime = 0f;
        while (_currentTime < taskTime)
        {
            _currentTime += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        FinishTask();
        _taskCoroutine = null;
    }
}
