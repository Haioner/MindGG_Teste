public class SoundDesigner : Employee
{
    private GameMakerController _gameMakerController;

    public override void Start()
    {
        base.Start();
        _gameMakerController = GameMakerController.instance;
    }

    public override void FinishTask()
    {
        base.FinishTask();
        _gameMakerController.GetGameStatistics().SoundValue += _employeeValue;
    }
}
