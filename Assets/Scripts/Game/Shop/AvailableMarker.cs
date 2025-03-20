using System.Numerics;
using UnityEngine.UI;
using UnityEngine;

public class AvailableMarker : MonoBehaviour
{
    [SerializeField] private Image availableMarkerImage;
    [SerializeField] private ShopItem shopItem;
    private ShopManager _shopManager;

    private void Start()
    {
        _shopManager = FindFirstObjectByType<ShopManager>();
        _shopManager.iCoins.OnAddCoin += UpdateMarker;
        UpdateMarker(_shopManager.iCoins.GetCoins());
    }

    private void OnDisable()
    {
        _shopManager.iCoins.OnAddCoin -= UpdateMarker;
    }

    private void UpdateMarker(BigInteger coins)
    {
        availableMarkerImage.enabled = coins >= shopItem.GetMinimalPrice(1);
    }
}
