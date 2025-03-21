using UnityEngine.UI;
using UnityEngine;

public class AvailableMarker : MonoBehaviour
{
    [SerializeField] private Image availableMarkerImage;
    [SerializeField] private ShopItem shopItem;
    private ShopManager _shopManager;

    private void Awake()
    {
        _shopManager = FindFirstObjectByType<ShopManager>();
    }

    private void Start()
    {
        _shopManager.iCoins.OnAddCoin += UpdateMarker;
        shopItem.OnPurchaseItem += UpdateMarker;
        UpdateMarker(_shopManager.iCoins.GetCoins());
    }

    private void OnDisable()
    {
        _shopManager.iCoins.OnAddCoin -= UpdateMarker;
        shopItem.OnPurchaseItem -= UpdateMarker;
    }

    private void UpdateMarker(int addLevel, bool isPurchased)
    {
        UpdateMarker(_shopManager.iCoins.GetCoins());
    }

    private void UpdateMarker(double coins)
    {
        bool isBelowMaxLevel = shopItem.GetCurrentLevel() < shopItem.GetMaxLevel();
        bool hasEnoughCoins = coins >= shopItem.GetMinimalPrice(1);
        availableMarkerImage.enabled = isBelowMaxLevel && hasEnoughCoins;
    }
}
