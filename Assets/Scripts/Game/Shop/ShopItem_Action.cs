using UnityEngine;

public abstract class ShopItem_Action : MonoBehaviour
{
    protected ShopItem _shopItem;
    private ObjectPool _floatNumberObjectPool;

    public virtual void Awake()
    {
        _shopItem = GetComponent<ShopItem>();
        _floatNumberObjectPool = GameObject.FindGameObjectWithTag("FloatNumberObjectPool").GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        _shopItem.OnPurchaseItem += OnPurchase;
        _shopItem.OnFailBuy += NotEnough_FloatNumber;
    }

    private void OnDisable()
    {
        _shopItem.OnPurchaseItem -= OnPurchase;
        _shopItem.OnFailBuy += NotEnough_FloatNumber;
    }

    public virtual void OnPurchase(int addLevel, bool isPurchased) { }

    public virtual void NotEnough_FloatNumber()
    {
        Vector2 randomOffset = Random.insideUnitCircle * 0.2f;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        GameObject floatNumber = _floatNumberObjectPool.GetObject_SetPosAndRot(spawnPosition, Quaternion.identity);
        floatNumber.GetComponent<FloatNumber>().InitFloatNumber("Not enough money!", _floatNumberObjectPool, Color.red);
    }
}
