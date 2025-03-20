using System.Numerics;

public interface ICoins
{
    public BigInteger GetCoins();
    public void ChangeCoins(BigInteger amount);
    public event System.Action<BigInteger> OnAddCoin;
}
