using UnityEngine;

public class Programmer : Employee
{
    [SerializeField] private float programmerValue = 10f;

    public override void FinishTask()
    {
        base.FinishTask();
        GameMakerController.instance.GetGameStatistics().ProgrammingValue += programmerValue;
    }
}
