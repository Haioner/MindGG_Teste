using UnityEngine;

[CreateAssetMenu(fileName = "ProgrammerData", menuName = "Employees/Programmer")]
public class ProgrammerSO : EmployeeSO
{
    [Header("Programmer")]
    public int BugFixes = 1;
    public float ChanceToFixBug = 0.5f;
    public float BugFixRate = 1f;

    [Header("Bug Upgrade")]
    public int PerLevel_BugFixes = 15;
    public float MaxChanceToFixBug = 0.01f;
    public float AddChanceToFixBug = 0.01f;
}