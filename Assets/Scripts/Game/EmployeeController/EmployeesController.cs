using System.Collections.Generic;
using UnityEngine;

public class EmployeesController : MonoBehaviour
{
    [SerializeField] private EmployeeSO employeeData;
    [SerializeField] private Employee employeePrefab;
    [SerializeField] private float spawnXOffset = 1.5f;
    [SerializeField] private List<Employee> employees = new List<Employee>();

    private void Awake()
    {
        InitStaterEmployees();
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
                }
            }
        }
    }

    public EmployeeSO EmployeeSO => employeeData;
    public int GetEmployeeCount() => employees.Count;

    public void AddNewEmployee()
    {
        if(employees.Count >= employeeData.MaxEmployeesCount)
        {
            Debug.LogWarning("Max employees reached!");
            return;
        }

        float xOffset = employees.Count * spawnXOffset;
        Vector3 position = new Vector3(xOffset, 0f, 0f);

        Employee newEmployee = Instantiate(employeePrefab, transform);
        newEmployee.transform.localPosition = position;
        newEmployee.EmployeeData = employeeData;

        employees.Add(newEmployee);
    }

    public void StartEmployeesTask()
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
