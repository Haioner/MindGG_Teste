using UnityEngine.UI;
using UnityEngine;
using TMPro;

public partial class EmployeeStatsCanvas : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image employeeImage;
    [SerializeField] private TextMeshProUGUI employeeNameText;
    [SerializeField] protected TextMeshProUGUI statsText;

    protected EmployeesController _employeesController;
    protected ShopItem _shopItem;
    protected int _levelAdd = 1;

    public void Start()
    {
        _shopItem = GetComponent<ShopItem>();
        _employeesController = GetComponentInParent<EmployeesController>();

        _shopItem.OnPurchaseItem += UpdateEmployeeStats;
        _shopItem.OnChangeLevelAdd += OnUpdateLevelAdd;

        UpdateEmployeeStats(_shopItem.GetCurrentLevel(), _shopItem.isPurchased);
    }

    private void OnDisable()
    {
        _shopItem.OnPurchaseItem -= UpdateEmployeeStats;
        _shopItem.OnChangeLevelAdd -= OnUpdateLevelAdd;
    }

    private void OnUpdateLevelAdd(int levelAdd)
    {
        _levelAdd = levelAdd;
        UpdateEmployeeStats(_shopItem.GetCurrentLevel(), _shopItem.isPurchased);
    }

    public virtual void UpdateEmployeeStats(int itemLevel, bool isPurchased)
    {
        EmployeeSO employeeSO = _employeesController.EmployeeSO;
        employeeImage.sprite = employeeSO.EmployeeSprite;
        employeeNameText.text = employeeSO.EmployeeName;

        int currentEmployeeValue = (int)_employeesController.GetFirstEmployeeValue();
        int additionalValue = _shopItem.GetCurrentLevel() < _shopItem.GetMaxLevel() ? _levelAdd : 0;

        int currentLevel = _shopItem.GetCurrentLevel();
        int newLevel = currentLevel + _levelAdd;
        int employeesToAdd = 0;

        if (currentLevel < _shopItem.GetMaxLevel())
        {
            employeesToAdd = (newLevel / employeeSO.PerLevel_AddEmployeesCount) - (currentLevel / employeeSO.PerLevel_AddEmployeesCount);
            employeesToAdd = Mathf.Clamp(employeesToAdd, 0, employeeSO.MaxEmployeesCount - _employeesController.GetEmployeeCount());
        }

        float timeReduction = 0;
        if (currentLevel < _shopItem.GetMaxLevel())
        {
            int reduceTaskTimeCount = (newLevel / employeeSO.PerLevel_ReduceTaskTime) - (currentLevel / employeeSO.PerLevel_ReduceTaskTime);
            reduceTaskTimeCount = Mathf.Max(reduceTaskTimeCount, 0);
            timeReduction = reduceTaskTimeCount * employeeSO.ReduceTaskTimeRate;
            timeReduction = Mathf.Min(timeReduction, 4);
        }

        bool canReduceTime = _employeesController.GetFirstEmployeeTaskTime() > 1;

        statsText.text = $"Expirience: {NumberConverter.ConvertNumberToString(currentEmployeeValue.ToString())}" +
                         (additionalValue > 0 ? $"<color=green> (+{NumberConverter.ConvertNumberToString(additionalValue.ToString())})</color>" : "") +
                         $"\nTask Time: {_employeesController.GetFirstEmployeeTaskTime():F2}" +
                         (canReduceTime && timeReduction > 0 ? $"<color=green> (-{timeReduction:F2})</color>" : "") +
                         $"\nEmployees: {_employeesController.GetEmployeeCount()}/{employeeSO.MaxEmployeesCount}" +
                         (employeesToAdd > 0 ? $"<color=green> (+{employeesToAdd})</color>" : "");
    }
}
