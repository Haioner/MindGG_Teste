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
                    _gameMakerController.GetGameStatistics().BugsValue -= programmerSO.BugFixes;
                }
                yield return new WaitForSeconds(programmerSO.BugFixRate);
            }
        }
        else
        {
            Debug.LogError("EmployeeData is not of type ProgrammerSO!");
        }
    }
    #endregion
}
