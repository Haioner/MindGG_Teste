using UnityEngine;

[CreateAssetMenu(fileName = "EmployeeData", menuName = "Employees/EmployeeData")]
public class EmployeeSO : ScriptableObject
{
    [Header("Employee")]
    public string EmployeeName;
    public Sprite EmployeeSprite;
    public float StartEmployeeValue = 10f;
    public float StartTaskTime = 5f;

    [Header("Task Upgrade")]
    public int PerLevel_ReduceTaskTime = 10;
    public float MinTaskTime = 1f;
    public float ReduceTaskTimeRate = 0.1f;

    [Header("Employees Count")]
    public int MaxEmployeesCount = 10;
    public int PerLevel_AddEmployeesCount = 15;
}

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