using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private string itemID;
    public bool isPurchased { get; set; }

    [Header("Level")]
    [Tooltip("-1 To disable level")]
    [SerializeField] private int itemLevel = 1;
    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int levelAdd = 1;

    [Header("Price")]
    [SerializeField] private float initialPrice;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private List<TextMeshProUGUI> levelText;
    [SerializeField] private TextMeshProUGUI levelUpText;

    private ShopManager _shopManager;
    private bool _isMaxSelected;
    private Coroutine _maxLevelUpgradeCoroutine;

    public event System.Action<string> OnFailBuy;
    public event System.Action<int, bool> OnPurchaseItem;
    public event System.Action<int> OnChangeLevelAdd;

    private void Awake()
    {
        if(string.IsNullOrEmpty(itemID))
            itemID = gameObject.name;

        _shopManager = FindFirstObjectByType<ShopManager>();

        upgradeButton.onClick.AddListener(OnClick);
        UpdateItem();
    }

    private void Start()
    {
        _shopManager.iCoins.OnAddCoin += SetMaxLevelUpgrade_CALLBACK;
    }

    private void OnDisable()
    {
        _shopManager.iCoins.OnAddCoin -= SetMaxLevelUpgrade_CALLBACK;
    }

    public string GetItemID() => itemID;
    public int GetCurrentLevel() => itemLevel;
    public int GetMaxLevel() => maxLevel;
    public int GetLevelAdd() => levelAdd;
    public void SetInitialPrice(float newInitialPrice) { initialPrice = newInitialPrice; UpdateItem(); }

    public void UpdateItem()
    {
        SetPriceText();
        SetLevelText();
        SetLevelUpText();
        UpdateUnlocked();
    }

    private void OnClick()
    {
        if (GetPrice() <= _shopManager.iCoins.GetCoins() && GetPrice() != 0 && itemLevel < maxLevel && !isPurchased)
        {
            int previousLevel = itemLevel;
            _shopManager.iCoins.ChangeCoins(-GetPrice());
            Unlock();
            LevelUp();
            UpdateItem();
            OnPurchaseItem?.Invoke(itemLevel - previousLevel, isPurchased);
            return;
        }

        if (GetPrice() > _shopManager.iCoins.GetCoins() || (GetPrice() == 0 && itemLevel < maxLevel))
            OnFailBuy?.Invoke("Not enough money!");
        else
            OnFailBuy?.Invoke("Already on MAX!");
    }

    private void LevelUp()
    {
        if (itemLevel == -1) return;

        if (itemLevel < maxLevel)
            itemLevel += levelAdd;

        if(itemLevel >= maxLevel)
        {
            itemLevel = maxLevel;
            levelAdd = 0;
        }
    }

    private void Unlock()
    {
        if (itemLevel == -1 && !isPurchased)
            isPurchased = true;
    }

    #region Price
    public void SetLevelUpgrade(int value)
    {
        if (itemLevel == -1 || itemLevel >= maxLevel) return;
        levelAdd = value;
        UpdateItem();
        OnChangeLevelAdd?.Invoke(levelAdd);
        SetMaxSelected(false);
    }

    public void SetMaxSelected(bool state) { _isMaxSelected = state; }

    public void SetMaxLevelUpgrade_CALLBACK(double coins)
    {
        if (_isMaxSelected)
        {
            if (_maxLevelUpgradeCoroutine != null)
                StopCoroutine(_maxLevelUpgradeCoroutine);

            _maxLevelUpgradeCoroutine = StartCoroutine(DelayedSetMaxLevelUpgrade());
        }
    }

    private System.Collections.IEnumerator DelayedSetMaxLevelUpgrade()
    {
        yield return new WaitForSeconds(0.1f);
        SetMaxLevelUpgrade();
    }

    public void SetMaxLevelUpgrade()
    {
        if (itemLevel == -1 || itemLevel >= maxLevel) return;

        int affordableLevels = 0;
        double cumulativePrice = 0;
        double playerCoins = _shopManager.iCoins.GetCoins();

        for (int i = itemLevel; i < maxLevel; i++)
        {
            double nextLevelPrice = GetMinimalPrice(i);
            if (cumulativePrice + nextLevelPrice > playerCoins)
                break;

            cumulativePrice += nextLevelPrice;
            affordableLevels++;
        }

        levelAdd = affordableLevels;
        UpdateItem();
        OnChangeLevelAdd?.Invoke(levelAdd);
    }

    public double GetMinimalPrice(int level)
    {
        return (double)Mathf.Floor(initialPrice * _shopManager.PriceMultiplier *  level);
    }

    private double GetCumulativePrice(int startLevel, int endLevel)
    {
        double cumulativePrice = 0;
        for (int i = startLevel; i < endLevel; i++)
        {
            cumulativePrice += GetMinimalPrice(i);
        }
        return cumulativePrice;
    }

    private double GetPrice()
    {
        if (itemLevel == -1) return initialPrice;
        if (itemLevel >= maxLevel) return 0;

        int targetLevel = itemLevel + levelAdd;
        if (targetLevel > maxLevel) targetLevel = maxLevel;

        int startLevel = itemLevel;
        if (startLevel > targetLevel) startLevel = targetLevel;

        return GetCumulativePrice(startLevel, targetLevel);
    }
    #endregion

    private void SetPriceText() => priceText.text = NumberConverter.ConvertNumberToString(GetPrice().ToString(), true);
    private void SetLevelText()
    {
        if(levelText.Count <= 0) return;
        foreach (var text in levelText)
        {
            text.text = "Lvl " + NumberConverter.ConvertNumberToString(itemLevel.ToString(), false);
        }
    }
    private void SetLevelUpText() { if(levelUpText != null) levelUpText.text = "Lvl Up x" + NumberConverter.ConvertNumberToString(levelAdd.ToString(), false); }
    public void UpdateUnlocked() { if(itemLevel == -1 && levelText.Count > 0) levelText[0].text = isPurchased ? "Purchased" : "Locked"; }
}
