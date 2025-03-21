using System.Collections;
using UnityEngine;

public class Programmer : Employee
{
    private GameMakerController _gameMakerController;
    private Coroutine _fixBugCoroutine;

    private float _currentBugFixChance;
    public float CurrentBugFixChance
    {
        get => _currentBugFixChance;
        set => _currentBugFixChance = value;
    }

    private int _currentBugFix;
    public int CurrentBugFix
    {
        get => _currentBugFix;
        set => _currentBugFix = value;
    }

    public override void Start()
    {
        base.Start();
        _gameMakerController = GameMakerController.instance;
    }

    private void OnEnable()
    {
        OnStartTaskEvent.AddListener(StartFixBugs);
        OnCompleteTaskEvent.AddListener(StopFixBugs);
    }

    private void OnDisable()
    {
        OnStartTaskEvent.RemoveListener(StartFixBugs);
        OnCompleteTaskEvent.RemoveListener(StopFixBugs);
    }

    public override void FinishTask()
    {
        base.FinishTask();
        _gameMakerController.GetGameStatistics().ProgrammingValue += _employeeValue;
    }

    #region Fix Bugs
    private void StartFixBugs()
    {
        if (_fixBugCoroutine == null)
        {
            _fixBugCoroutine = StartCoroutine(FixBugsOverTime());
        }
    }

    private void StopFixBugs()
    {
        if (_fixBugCoroutine != null)
        {
            StopCoroutine(_fixBugCoroutine);
            _fixBugCoroutine = null;
        }
    }

    private IEnumerator FixBugsOverTime()
    {
        if (EmployeeData is ProgrammerSO programmerSO)
        {
            while (true)
            {
                float randFix = Random.value;
                if (_gameMakerController.GetGameStatistics().BugsValue > 0 && randFix <= _currentBugFixChance)
                {
                    _gameMakerController.GetGameStatistics().BugsValue -= _currentBugFix;
                    CreateBugFixFloatNumber();
                }
                yield return new WaitForSeconds(programmerSO.BugFixRate);
            }
        }
        else
        {
            Debug.LogError("EmployeeData is not of type ProgrammerSO!");
        }
    }

    private void CreateBugFixFloatNumber()
    {
        Vector2 randomOffset = Random.insideUnitCircle * floatNumberRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        GameObject floatNumber = floatNumberObjectPool.GetObject_SetPosAndRot(spawnPosition, Quaternion.identity);
        floatNumber.GetComponent<FloatNumber>().InitFloatNumber($"BugFixed: {_currentBugFix}", floatNumberObjectPool, floatNumberColor);
    }
    #endregion
}
