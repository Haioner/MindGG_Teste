using UnityEngine;

public abstract class ShopItem_Action : MonoBehaviour
{
    protected ShopItem shopItem;

    private void Awake()
    {
        shopItem = GetComponent<ShopItem>();
    }

    private void OnEnable()
    {
        shopItem.OnPurchaseItem += OnPurchase;
    }

    private void OnDisable()
    {
        shopItem.OnPurchaseItem -= OnPurchase;
    }

    public virtual void OnPurchase(int itemLevel, bool isPurchased) { }
}
