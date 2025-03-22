using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CoinsController : MonoBehaviour, ICoins
{
    [SerializeField] private TextMeshProUGUI coinText;
    public double AccumulatedCoins { get; private set; }
    private double _coins;

    [SerializeField] private UnityEvent OnADDCoinEvent;
    public event System.Action<double> OnChangeCoins;

    private void Start()
    {
        InitCoins();
        //SetCoins();
    }

    private void InitCoins()
    {
        UpdateCoinText();
        OnChangeCoins?.Invoke(_coins);
    }

    public double GetCoins()
    {
        return _coins;
    }

    public void ChangeCoins(double amount)
    {
        _coins += amount;
        AccumulatedCoins += amount;
        UpdateCoinText();
        Debug.Log($"Coins Added: {amount}");

        if (amount > 0)
            OnADDCoinEvent?.Invoke();

        OnChangeCoins?.Invoke(_coins);
    }

    private void UpdateCoinText()
    {
        coinText.text = "<sprite=0>" + NumberConverter.ConvertNumberToString(_coins.ToString(), false);
    }

    [ContextMenu("AddCoins")]
    public void SetCoins()
    {
        string LargeConstant = "9999999999999999999999999999999999";
        ChangeCoins(double.Parse(LargeConstant));
    }
}
