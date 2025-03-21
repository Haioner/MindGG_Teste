using UnityEngine;

public class ProgrammerStatsCanvas : EmployeeStatsCanvas
{
    public override void UpdateEmployeeStats(int itemLevel, bool isPurchased)
    {
        base.UpdateEmployeeStats(itemLevel, isPurchased);

        EmployeeSO employeeSO = _employeesController.EmployeeSO;
        int currentLevel = _shopItem.GetCurrentLevel();
        int newLevel = currentLevel + _levelAdd;
        string statsString = statsText.text;

        if (employeeSO is ProgrammerSO programmerSO)
        {
            float currentBugFixChance = _employeesController.GetFirstEmployeeChanceToFixBug();
            float additionalBugFixChance = 0;

            if (currentLevel < _shopItem.GetMaxLevel())
            {
                int bugFixIncreases = (newLevel / programmerSO.PerLevel_BugFixes) - (currentLevel / programmerSO.PerLevel_BugFixes);
                additionalBugFixChance = bugFixIncreases * programmerSO.AddChanceToFixBug;
                additionalBugFixChance = Mathf.Min(additionalBugFixChance, programmerSO.MaxChanceToFixBug - currentBugFixChance);
            }


            statsString += $"\nBug Fix Chance: {currentBugFixChance:P2}" +
                           (additionalBugFixChance > 0 ? $"<color=green> (+{additionalBugFixChance:P2})</color>" : "");
        }

        statsText.text = statsString;
    }
}

