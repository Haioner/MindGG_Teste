public interface ICoins
{
    public double GetCoins();
    public void ChangeCoins(double amount);
    public event System.Action<double> OnChangeCoins;
}
