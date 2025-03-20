using UnityEngine;

public class SoundDesigner : Employee
{
    private GameMakerController _gameMakerController;

    private void Start()
    {
        _gameMakerController = GameMakerController.instance;
    }

    public override void FinishTask()
    {
        base.FinishTask();
        _gameMakerController.GetGameStatistics().SoundValue += employeeValue;
    }
}
