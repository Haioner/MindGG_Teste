using UnityEngine;
using TMPro;

public class CoinsController : MonoBehaviour, ICoins
{
    public float AccumulatedCoins { get; private set; }
    [SerializeField] private TextMeshProUGUI coinText;
    private float _coins;

    public event System.Action OnAddCoin;

    private void Start()
    {
        InitCoins();
    }

    private void InitCoins()
    {
        coinText.text = NumberConverter.ConvertNumberToString(_coins, false);
        OnAddCoin?.Invoke();
    }

    public float GetCoins()
    {
        return _coins;
    }

    public void ChangeCoins(float amount)
    {
        _coins += Mathf.FloorToInt(amount);
        AccumulatedCoins += Mathf.FloorToInt(amount);
        coinText.text = NumberConverter.ConvertNumberToString(_coins, false);
        OnAddCoin?.Invoke();
    }
}
