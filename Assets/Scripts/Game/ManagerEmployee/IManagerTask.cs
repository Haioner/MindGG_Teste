public interface IManagerTask
{
    public void StartTask();
    public event System.Action OnTaskFinished;
}
