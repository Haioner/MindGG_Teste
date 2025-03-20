using System.Collections.Generic;
using UnityEngine;

public class EmployeesController : MonoBehaviour
{
    [SerializeField] private Employee employeePrefab;
    [SerializeField] private List<Employee> employees = new List<Employee>();

    public void AddNewEmployee()
    {
        var newEmployee = Instantiate(employeePrefab, transform);
        employees.Add(newEmployee);
    }

    public void StartEmployeesTask()
    {
        foreach (var employee in employees)
        {
            employee.StartTask();
        }
    }
}
