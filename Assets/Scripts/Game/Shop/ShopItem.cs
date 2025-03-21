using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Numerics;
using System.Collections.Generic;

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

    public string GetItemID() => itemID;
    public int GetCurrentLevel() => itemLevel;
    public int GetMaxLevel() => maxLevel;
    public int GetLevelAdd() => levelAdd;

    public void UpdateItem()
    {
        SetPriceText();
        SetLevelText();
        SetLevelUpText();
        UpdateUnlocked();
    }

    private void OnClick()
    {
        if(GetPrice() <= _shopManager.iCoins.GetCoins() && itemLevel < maxLevel && !isPurchased)
        {
            int previousLevel = itemLevel;
            _shopManager.iCoins.ChangeCoins(-GetPrice());
            Unlock();
            LevelUp();
            UpdateItem();
            OnPurchaseItem?.Invoke(itemLevel - previousLevel, isPurchased);
        }
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
    }

    public void SetMaxLevelUpgrade()
    {
        if (itemLevel == -1 || itemLevel >= maxLevel) return;
        levelAdd = maxLevel - itemLevel;
        UpdateItem();
        OnChangeLevelAdd?.Invoke(levelAdd);
    }

    public BigInteger GetMinimalPrice(int level)
    {
        return (BigInteger)Mathf.Floor(initialPrice * _shopManager.PriceMultiplier *  level);
    }

    private BigInteger GetCumulativePrice(int startLevel, int endLevel)
    {
        BigInteger cumulativePrice = 0;
        for (int i = startLevel; i < endLevel; i++)
        {
            cumulativePrice += GetMinimalPrice(i);
        }
        return cumulativePrice;
    }

    private BigInteger GetPrice()
    {
        if (itemLevel == -1 || itemLevel >= maxLevel) return 0;

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
        foreach (var text in levelText)
        {
            text.text = "Lvl " + NumberConverter.ConvertNumberToString(itemLevel.ToString(), false);
        }
    }
    private void SetLevelUpText() { if(levelUpText != null) levelUpText.text = "Lvl Up x" + NumberConverter.ConvertNumberToString(levelAdd.ToString(), false); }
    public void UpdateUnlocked() { if(itemLevel == -1) levelText[0].text = isPurchased ? "Purchased" : "Locked"; }
}
