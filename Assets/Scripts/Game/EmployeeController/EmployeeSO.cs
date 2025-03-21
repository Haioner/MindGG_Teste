using UnityEngine;

[CreateAssetMenu(fileName = "EmployeeData", menuName = "Employees/EmployeeData")]
public class EmployeeSO : ScriptableObject
{
    [Header("Employee")]
    public string EmployeeName;
    public Sprite EmployeeSprite;
    public float StartEmployeeValue = 10f;
    public float StartTaskTime = 5f;
    public Employee employeePrefab;

    [Header("Task Upgrade")]
    public int PerLevel_ReduceTaskTime = 10;
    public float MinTaskTime = 1f;
    public float ReduceTaskTimeRate = 0.1f;

    [Header("Employees Count")]
    public int MaxEmployeesCount = 10;
    public int PerLevel_AddEmployeesCount = 15;
}
