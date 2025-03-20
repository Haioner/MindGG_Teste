using UnityEngine;

public class Marketer : Employee
{
    private GameMakerController _gameMakerController;

    private void Start()
    {
        _gameMakerController = GameMakerController.instance;
    }

    public override void FinishTask()
    {
        base.FinishTask();
        _gameMakerController.GetGameStatistics().MarketingValue += employeeValue;
    }
}