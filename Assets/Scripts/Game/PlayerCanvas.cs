using UnityEngine;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;

    private ShopItem _shopItem;
    private PlayerManager _playerManager;

    private void Awake()
    {
        _shopItem = GetComponent<ShopItem>();
        _shopItem.OnPurchaseItem += UpdateUI;
    }

    private void Start()
    {
        _playerManager = PlayerManager.instance;

        UpdateUI(_shopItem.GetCurrentLevel(), _shopItem.isPurchased);
        _shopItem.OnChangeLevelAdd += UpdateOnLevelAdd;
    }

    private void OnDisable()
    {
        _shopItem.OnPurchaseItem -= UpdateUI;
        _shopItem.OnChangeLevelAdd -= UpdateOnLevelAdd;
    }

    private void UpdateOnLevelAdd(int levelAdd)
    {
        UpdateUI(levelAdd, _shopItem.isPurchased);
    }

    private void UpdateUI(int levelAdd, bool isPurchased)
    {
        float currentProgressMultiplier = _playerManager.GameProgressSpeedMultiplier;
        float additionalValue = _playerManager.CalculateAdditionalValue(levelAdd);

        statsText.text = $"Progress Multiplier: {currentProgressMultiplier:F2}" +
                         (additionalValue > 0 ? $"<color=green> (+{additionalValue:F2})</color>" : "");
    }
}
