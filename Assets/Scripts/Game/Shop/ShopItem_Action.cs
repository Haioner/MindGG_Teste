using UnityEngine;
using UnityEngine.Events;

public abstract class ShopItem_Action : MonoBehaviour
{
    protected ShopItem _shopItem;
    private ObjectPool _floatNumberObjectPool;
    [SerializeField] protected UnityEvent OnPurchaseEvent;

    public virtual void Awake()
    {
        _shopItem = GetComponent<ShopItem>();
        _floatNumberObjectPool = GameObject.FindGameObjectWithTag("FloatNumberObjectPool").GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        _shopItem.OnPurchaseItem += OnPurchase;
        _shopItem.OnFailBuy += FailBuy_FloatNumber;
    }

    private void OnDisable()
    {
        _shopItem.OnPurchaseItem -= OnPurchase;
        _shopItem.OnFailBuy += FailBuy_FloatNumber;
    }

    public virtual void OnPurchase(int addLevel, bool isPurchased) { OnPurchaseEvent?.Invoke(); }

    public virtual void FailBuy_FloatNumber(string message)
    {
        Vector2 randomOffset = Random.insideUnitCircle * 0.2f;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        GameObject floatNumber = _floatNumberObjectPool.GetObject_SetPosAndRot(spawnPosition, Quaternion.identity);
        floatNumber.GetComponent<FloatNumber>().InitFloatNumber(message, _floatNumberObjectPool, Color.red);
    }
}
