using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [HideInInspector] public EmployeeSO EmployeeData;

    [Header("Task")]
    [SerializeField] protected float timeMultiplier = 1f;
    protected float _currentTime = 0f;

    protected float _currentTaskTime;
    public float CurrentTaskTime
    {
        get => _currentTaskTime;
        set => _currentTaskTime = value;
    }

    [Space][SerializeField] protected UnityEvent OnStartTaskEvent;
    [Space][SerializeField] protected UnityEvent OnCompleteTaskEvent;
    public event System.Action<float, float> OnTaskTimeChanged;
    private Coroutine _taskCoroutine;

    protected float _employeeValue;
    public float EmployeeValue
    {
        get => _employeeValue;
        set => _employeeValue = value;
    }

    protected EmployeesController employeesController;

    public virtual void Start()
    {
        employeesController = GetComponentInParent<EmployeesController>();
    }

    private void OnMouseDown()
    {
        StartTask();
        employeesController.StartEmployeesTask();
    }

    public void ReduceTaskTime(float timeReduction)
    {
        _currentTaskTime = Mathf.Max(_currentTaskTime - timeReduction, EmployeeData.MinTaskTime);
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
        OnTaskTimeChanged?.Invoke(_currentTaskTime, timeMultiplier);
        _currentTime = 0f;
        while (_currentTime < _currentTaskTime)
        {
            _currentTime += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        FinishTask();
        _taskCoroutine = null;
    }
}
