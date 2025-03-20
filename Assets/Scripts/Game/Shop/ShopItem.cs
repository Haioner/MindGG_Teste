using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private string itemID;
    public bool isPurchased { get; set; }

    [Header("Level")]
    [Tooltip("-1 To disable level")]
    [SerializeField] private int itemLevel = 1;
    [SerializeField] private int maxLevel = 10;

    [Header("Price")]
    [SerializeField] private float initialPrice;
    private float _currentPrice;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI levelText;

    private ShopManager _shopManager;

    public event System.Action<int, bool> OnPurchaseItem;

    private void Awake()
    {
        if(string.IsNullOrEmpty(itemID))
            itemID = gameObject.name;

        _shopManager = FindFirstObjectByType<ShopManager>();

        GetComponent<Button>().onClick.AddListener(OnClick);
        UpdateItem();
    }

    public string GetItemID() => itemID;

    private void OnClick()
    {
        if(_currentPrice <= _shopManager.iCoins.GetCoins() && itemLevel < maxLevel && !isPurchased)
        {
            _shopManager.iCoins.ChangeCoins(-_currentPrice);
            Unlock();
            LevelUp();
            UpdateItem();
            OnPurchaseItem?.Invoke(itemLevel, isPurchased);
        }
    }

    private void LevelUp()
    {
        if (itemLevel == -1) return;

        if (itemLevel < maxLevel)
            itemLevel++;
    }

    private void Unlock()
    {
        if (itemLevel == -1 && !isPurchased)
            isPurchased = true;
    }

    public void UpdateItem()
    {
        UpdatePrice();
        SetPriceText();
        SetLevelText();
        UpdateUnlocked();
    }

    private void UpdatePrice() => _currentPrice = Mathf.RoundToInt(initialPrice * Mathf.Pow(_shopManager.PriceMultiplier, itemLevel - 1));
    private void SetPriceText() => priceText.text = NumberConverter.ConvertNumberToString(_currentPrice, false);
    private void SetLevelText() => levelText.text = "Lvl " + NumberConverter.ConvertNumberToString(itemLevel, false);
    public void UpdateUnlocked() { if(itemLevel == -1) levelText.text = isPurchased ? "Purchased" : "Locked"; }
}
