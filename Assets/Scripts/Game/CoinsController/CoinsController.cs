using UnityEngine;
using TMPro;

public class CoinsController : MonoBehaviour, ICoins
{
    [SerializeField] private TextMeshProUGUI coinText;
    public double AccumulatedCoins { get; private set; }
    private double _coins;

    public event System.Action<double> OnAddCoin;

    private void Start()
    {
        InitCoins();
        SetCoins();
    }

    private void InitCoins()
    {
        coinText.text = NumberConverter.ConvertNumberToString(_coins.ToString(), false);
        OnAddCoin?.Invoke(_coins);
    }

    public double GetCoins()
    {
        return _coins;
    }

    public void ChangeCoins(double amount)
    {
        _coins += amount;
        AccumulatedCoins += amount;
        coinText.text = NumberConverter.ConvertNumberToString(_coins.ToString(), false);
        Debug.Log($"Coins Added: {amount}");
        OnAddCoin?.Invoke(_coins);
    }

    [ContextMenu("AddCoins")]
    public void SetCoins()
    {
        string LargeConstant = "9999999999999999999999999999999999";
        ChangeCoins(double.Parse(LargeConstant));
    }
}
