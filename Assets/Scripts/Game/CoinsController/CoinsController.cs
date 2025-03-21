using System.Numerics;
using UnityEngine;
using TMPro;

public class CoinsController : MonoBehaviour, ICoins
{
    [SerializeField] private TextMeshProUGUI coinText;
    public BigInteger AccumulatedCoins { get; private set; }
    private BigInteger _coins;

    public event System.Action<BigInteger> OnAddCoin;

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

    public BigInteger GetCoins()
    {
        return _coins;
    }

    public void ChangeCoins(BigInteger amount)
    {
        _coins += amount;
        AccumulatedCoins += amount;
        coinText.text = NumberConverter.ConvertNumberToString(_coins.ToString(), false);
        OnAddCoin?.Invoke(_coins);
    }

    [ContextMenu("AddCoins")]
    public void SetCoins()
    {
        string LargeConstant = "999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
        ChangeCoins(BigInteger.Parse(LargeConstant));
    }
}
