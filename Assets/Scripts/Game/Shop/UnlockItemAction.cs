using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class UnlockItemAction : ShopItem_Action
{
    [Header("Instantiate")]
    [SerializeField] private Transform parentTransform;
    [SerializeField] private List<GameObject> objectsToInstantiate = new List<GameObject>();

    [Header("SetActive")]
    [SerializeField] private List<GameObject> objectsToDeactive = new List<GameObject>();
    [SerializeField] private List<GameObject> objectsToActive = new List<GameObject>();

    [Header("Event")]
    [SerializeField] private UnityEvent onPurchaseEvent;
    [SerializeField] private UnityEvent onSellEvent;

    public override void OnPurchase(int addLevel, bool isPurchased)
    {
        base.OnPurchase(addLevel, isPurchased);

        foreach (var obj in objectsToInstantiate)
        {
            if(parentTransform == null)
                Instantiate(obj);
            else
                Instantiate(obj, parentTransform);
        }

        foreach (var obj in objectsToActive)
            obj.SetActive(true);

        foreach (var obj in objectsToDeactive)
            obj.SetActive(false);

        onPurchaseEvent?.Invoke();
    }

    public void Sell()
    {
        foreach (var obj in objectsToDeactive)
            obj.SetActive(true);

        foreach (var obj in objectsToActive)
            obj.SetActive(false);

        _shopItem.isPurchased = false;
        onSellEvent?.Invoke();
    }
}
