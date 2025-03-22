using System.Collections.Generic;
using UnityEngine;

public class EmployeesController : MonoBehaviour, IManagerTask
{
    [Header("Employee Data")]
    [SerializeField] private EmployeeSO employeeData;

    [Header("Instantiate")]
    [SerializeField] private int startEmployeesCount = 0;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnXOffset = 1.5f;
    [SerializeField] private List<Employee> employees = new List<Employee>();

    [Header("Unlock")]
    [SerializeField] private bool startLocked = true;
    [SerializeField] private GameObject unlockObject, employeeUpgrade;

    public event System.Action OnEmployeeTaskFinished;
    public event System.Action OnTaskFinished;

    private void Start()
    {
        StartLocked();
        InstantiateStarterEmployees();
        InitStaterEmployees();
    }

    public void InitEmployeeData(EmployeeControllerData employeeControllerData)
    {
        employeeData = employeeControllerData.EmployeeData;
        startEmployeesCount = employeeControllerData.StartEmployeesCount;
        startLocked = employeeControllerData.StartLocked;
        unlockObject.GetComponent<ShopItem>().SetInitialPrice(employeeControllerData.InitialUnlockPrice);
    }

    private void StartLocked()
    {
        if (startLocked)
        {
            unlockObject.SetActive(true);
            employeeUpgrade.SetActive(false);
        }
        else
            unlockObject.GetComponent<UnlockItemAction>().OnPurchase(default, default);
    }

    public void CreateFirstEmployee()
    {
        if (startEmployeesCount > 0) return;
        AddNewEmployee();
        InitStaterEmployees();
    }

    private void InstantiateStarterEmployees()
    {
        if (startEmployeesCount > 0 && !startLocked)
        {
            for (int i = 0; i < startEmployeesCount; i++)
            {
                AddNewEmployee();
            }
        }
    }

    public void InitStaterEmployees()
    {
        if (employees.Count > 0)
        {
            foreach (var employee in employees)
            {
                employee.EmployeeData = employeeData;
                employee.CurrentTaskTime = employeeData.StartTaskTime;
                employee.EmployeeValue = employeeData.StartEmployeeValue;

                if (employee is Programmer programmer)
                {
                    programmer.CurrentBugFixChance = ((ProgrammerSO)employeeData).ChanceToFixBug;
                    programmer.CurrentBugFix = ((ProgrammerSO)employeeData).BugFixes;
                }
            }
        }
    }

    public EmployeeSO EmployeeSO => employeeData;
    public int GetEmployeeCount() => employees.Count;
    public List<Employee> GetEmployees() => employees;

    public void AddNewEmployee()
    {
        if(employees.Count >= employeeData.MaxEmployeesCount)
        {
            Debug.LogWarning("Max employees reached!");
            return;
        }

        float xOffset = employees.Count * spawnXOffset;
        Vector3 spawnPosition = spawnPos.position + new Vector3(xOffset, 0f, 0f);
        Employee newEmployee = Instantiate(employeeData.employeePrefab, spawnPosition, Quaternion.identity, transform);
        newEmployee.EmployeeData = employeeData;

        employees.Add(newEmployee);
        newEmployee.OnFinishedTask += () => OnTaskFinished?.Invoke();
    }

    public void StartTask()
    {
        foreach (var employee in employees)
        {
            employee.StartTask();
        }
    }

    public float GetFirstEmployeeValue() 
    {
        if (employees.Count > 0) 
            return employees[0].EmployeeValue;
        return 0;
    }

    public void AddEmployeesValue(float value)
    {
        Employee firstEmployee = employees[0];
        firstEmployee.EmployeeValue += value;

        foreach (var employee in employees)
        {
            if (employee == firstEmployee) continue;
            employee.EmployeeValue = firstEmployee.EmployeeValue;
        }
    }

    public float GetFirstEmployeeTaskTime()
    {
        if (employees.Count > 0)
            return employees[0].CurrentTaskTime;
        return 0;
    }

    public void ReduceEmployeesTaskTime(float timeReduction)
    {
        Employee firstEmployee = employees[0];
        firstEmployee.ReduceTaskTime(timeReduction);

        foreach (var employee in employees)
        {
            if (employee == firstEmployee) continue;
            employee.CurrentTaskTime = firstEmployee.CurrentTaskTime;
        }
    }

    public float GetFirstEmployeeChanceToFixBug()
    {
        if (employees.Count > 0)
            return ((Programmer)employees[0])?.CurrentBugFixChance ?? 0f;
        return 0f;
    }

    public void IncreaseChanceToFixBug(float chanceIncrease)
    {
        if (employeeData is ProgrammerSO programmerSO)
        {
            Programmer firstProgrammer = (Programmer)employees[0];
            firstProgrammer.CurrentBugFixChance += chanceIncrease;

            foreach (var employee in employees)
            {
                if (employee is Programmer programmer)
                    programmer.CurrentBugFixChance = firstProgrammer.CurrentBugFixChance;
            }
        }
    }
}
