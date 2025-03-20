[System.Serializable]
public class GameStatistics
{
    private float programming;
    private float art;
    private float sound;
    private float marketing;
    private int bugs;

    public event System.Action OnValuesChanged;

    public float ProgrammingValue
    {
        get => programming;
        set
        {
            programming = value;
            OnValuesChanged?.Invoke();
        }
    }

    public float ArtValue
    {
        get => art;
        set
        {
            art = value;
            OnValuesChanged?.Invoke();
        }
    }

    public float SoundValue
    {
        get => sound;
        set
        {
            sound = value;
            OnValuesChanged?.Invoke();
        }
    }

    public float MarketingValue
    {
        get => marketing;
        set
        {
            marketing = value;
            OnValuesChanged?.Invoke();
        }
    }

    public int BugsValue
    {
        get => bugs;
        set
        {
            bugs = value;
            if(bugs < 0) bugs = 0;
            OnValuesChanged?.Invoke();
        }
    }

    public float GetMediumValue()
    {
        return (ProgrammingValue + ArtValue + SoundValue + MarketingValue) / 5;
    }

    public void ResetWhitoutEvent()
    {
        programming = 0;
        art = 0;
        sound = 0;
        marketing = 0;
    }

    public void Reset()
    {
        ResetWhitoutEvent();
        OnValuesChanged?.Invoke();
    }
}
