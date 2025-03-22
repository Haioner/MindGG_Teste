using UnityEngine;

public class PlayerUpgrade : ShopItem_Action
{
    public override void OnPurchase(int addLevel, bool isPurchased)
    {
        base.OnPurchase(addLevel, isPurchased);
        PlayerManager.instance.IncreaseGameProgressSpeed(addLevel);
    }
}
