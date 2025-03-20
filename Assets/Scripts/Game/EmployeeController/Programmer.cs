using System.Collections;
using UnityEngine;

public class Programmer : Employee
{
    [Header("Programmer")]
    [SerializeField] private float programmerValue = 10f;

    [Header("Bugs")]
    [SerializeField] private int bugFixes = 1;
    [SerializeField] private float chanceToFixBug = 0.5f;
    [SerializeField] private float bugFixRate = 1f;

    private GameMakerController _gameMakerController;
    private Coroutine _fixBugCoroutine;

    private void Start()
    {
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
        _gameMakerController.GetGameStatistics().ProgrammingValue += programmerValue;
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
        while (true)
        {
            float randFix = Random.value;
            if (_gameMakerController.GetGameStatistics().BugsValue > 0 && randFix <= chanceToFixBug)
            {
                _gameMakerController.GetGameStatistics().BugsValue -= bugFixes;
            }
            yield return new WaitForSeconds(bugFixRate);
        }
    }
    #endregion
}
