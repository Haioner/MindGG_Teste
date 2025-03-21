using UnityEngine;

public abstract class ShopItem_Action : MonoBehaviour
{
    protected ShopItem shopItem;

    public virtual void Awake()
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

    public virtual void OnPurchase(int addLevel, bool isPurchased) { }
}
