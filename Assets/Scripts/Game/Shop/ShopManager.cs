using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject coinsController;
    public ICoins iCoins;
    public float PriceMultiplier = 1.5f;

    private void Awake()
    {
        iCoins = coinsController.GetComponent<ICoins>();
    }
}
