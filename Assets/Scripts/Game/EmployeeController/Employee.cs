using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [HideInInspector] public EmployeeSO EmployeeData;

    [Header("Task")]
    [SerializeField] protected float timeMultiplier = 1f;
    protected float _currentTime = 0f;

    [Header("Tap Area GFX")]
    [SerializeField] private SpriteRenderer areaSpriteRenderer;

    [Header("FloatNumber")]
    [SerializeField] protected Color floatNumberColor = Color.white;
    [SerializeField] protected float floatNumberRadius = 0.5f;
    [SerializeField] protected Vector2 offsetSpawn;
    [SerializeField] protected string floatNumberPreffix = "";

    [Space][SerializeField] protected UnityEvent OnStartTaskEvent;
    [Space][SerializeField] protected UnityEvent OnCompleteTaskEvent;
    public event System.Action OnFinishedTask;
    public event System.Action<float, float> OnTaskTimeChanged;
    private Coroutine _taskCoroutine;

    protected float _currentTaskTime;
    public float CurrentTaskTime
    {
        get => _currentTaskTime;
        set => _currentTaskTime = value;
    }

    protected float _employeeValue;
    public float EmployeeValue
    {
        get => _employeeValue;
        set => _employeeValue = value;
    }

    protected EmployeesController employeesController;
    protected ObjectPool floatNumberObjectPool;

    public virtual void Awake()
    {
        floatNumberObjectPool = GameObject.FindGameObjectWithTag("FloatNumberObjectPool").GetComponent<ObjectPool>();
        employeesController = GetComponentInParent<EmployeesController>();
    }

    public virtual void Start()
    {

    }

    private void OnMouseDown()
    {
        StartTask();
        employeesController.StartTask();
    }

    public void ReduceTaskTime(float timeReduction)
    {
        _currentTaskTime = Mathf.Max(_currentTaskTime - timeReduction, EmployeeData.MinTaskTime);
    }

    public virtual void StartTask()
    {
        if(_taskCoroutine == null)
        {
            areaSpriteRenderer.enabled = false;
            _taskCoroutine = StartCoroutine(CalculateTaskTimer());
            OnStartTaskEvent?.Invoke();
        }
    }

    public virtual void FinishTask()
    {
        areaSpriteRenderer.enabled = true;
        CreateFloatNumber();
        OnFinishedTask?.Invoke();
        OnCompleteTaskEvent?.Invoke();
    }

    private IEnumerator CalculateTaskTimer()
    {
        OnTaskTimeChanged?.Invoke(_currentTaskTime, timeMultiplier);
        _currentTime = 0f;
        while (_currentTime < _currentTaskTime)
        {
            _currentTime += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        _taskCoroutine = null;
        FinishTask();
    }

    public virtual void CreateFloatNumber()
    {
        Vector2 randomOffset = Random.insideUnitCircle * floatNumberRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        spawnPosition.x += offsetSpawn.x;
        spawnPosition.y += offsetSpawn.y;
        GameObject floatNumber = floatNumberObjectPool.GetObject_SetPosAndRot(spawnPosition, Quaternion.identity);
        floatNumber.GetComponent<FloatNumber>().InitFloatNumber(floatNumberPreffix + FloatNumberText(), floatNumberObjectPool, floatNumberColor);
    }

    public virtual string FloatNumberText()
    {
        return NumberConverter.ConvertNumberToString(_employeeValue.ToString(), false);
    }
}
