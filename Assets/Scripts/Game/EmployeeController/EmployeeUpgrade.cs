using UnityEngine.Events;
using UnityEngine;

public class EmployeeUpgrade : ShopItem_Action
{
    protected EmployeesController employeesController;
    //[SerializeField] private UnityEvent OnPurchaseEvent;

    public override void Awake()
    {
        base.Awake();
        employeesController = GetComponentInParent<EmployeesController>();
    }

    public override void OnPurchase(int addLevel, bool isPurchased)
    {
        base.OnPurchase(addLevel, isPurchased);
        LevelUp_Employees(addLevel);
        OnPurchaseEvent?.Invoke();
    }

    public virtual void LevelUp_Employees(int addLevel)
    {
        EmployeeSO employeeSO = employeesController.EmployeeSO;

        AddNewEmployee(addLevel, employeeSO);
        AddEmployeeValue(addLevel);
        ReduceTaskTime(addLevel, employeeSO);
    }

    private void AddNewEmployee(int addLevel, EmployeeSO employeeSO)
    {
        if(employeesController.GetEmployeeCount() <= 0)
        {
            employeesController.AddNewEmployee();
            employeesController.InitStaterEmployees();
        }

        int currentLevel = _shopItem.GetCurrentLevel();
        int previousLevel = currentLevel - addLevel;
        int employeesToAdd = (currentLevel / employeeSO.PerLevel_AddEmployeesCount) - (previousLevel / employeeSO.PerLevel_AddEmployeesCount);

        for (int i = 0; i < employeesToAdd && employeesController.GetEmployeeCount() < employeeSO.MaxEmployeesCount; i++)
            employeesController.AddNewEmployee();
    }

    private void AddEmployeeValue(int addLevel)
    {
        employeesController.AddEmployeesValue(addLevel);
    }

    private void ReduceTaskTime(int addLevel, EmployeeSO employeeSO)
    {
        int currentLevel = _shopItem.GetCurrentLevel();
        int previousLevel = currentLevel - addLevel;
        int reduceTaskTimeCount = 0;

        for (int level = previousLevel + 1; level <= currentLevel; level++)
        {
            if (level > 0 && level % employeeSO.PerLevel_ReduceTaskTime == 0)
            {
                reduceTaskTimeCount++;
            }
        }

        float timeReduction = reduceTaskTimeCount * employeeSO.ReduceTaskTimeRate;
        employeesController.ReduceEmployeesTaskTime(timeReduction);
    }
}
