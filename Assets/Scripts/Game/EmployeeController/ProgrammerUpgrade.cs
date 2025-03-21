using UnityEngine;

public class ProgrammerUpgrade : EmployeeUpgrade
{
    public override void OnPurchase(int itemLevel, bool isPurchased)
    {
        LevelUp_Employees(itemLevel);
        LevelUp_Programmer(itemLevel);
    }

    private void LevelUp_Programmer(int addLevel)
    {
        if (employeesController.EmployeeSO is ProgrammerSO programmerSO)
        {
            int currentLevel = shopItem.GetCurrentLevel();
            int previousLevel = currentLevel - addLevel;
            int bugFixIncreases = 0;

            for (int level = previousLevel + 1; level <= currentLevel; level++)
            {
                if (level > 0 && level % programmerSO.PerLevel_BugFixes == 0)
                {
                    bugFixIncreases++;
                }
            }

            float chanceToFixBugIncrease = bugFixIncreases * programmerSO.AddChanceToFixBug;
            float currentBugFixChance = employeesController.GetFirstEmployeeChanceToFixBug();
            chanceToFixBugIncrease = Mathf.Min(chanceToFixBugIncrease, programmerSO.MaxChanceToFixBug - currentBugFixChance);

            employeesController.IncreaseChanceToFixBug(chanceToFixBugIncrease);
        }
    }
}